using Dapper;
using FinanceApp.Models;
using Microsoft.Data.SqlClient;

namespace FinanceApp.Services
{
    public interface IRepositoryAccounts
    {
        Task Create(Account account);
        Task<Account> GetById(int id, int userId);
        Task<IEnumerable<Account>> Search(int userId);
        Task Update(AccountCreationViewModel account);
    }
    public class RepositoryAccounts : IRepositoryAccounts
    {
        private readonly string connectionString;

        public RepositoryAccounts(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task Create(Account account)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Account (Name, AccountTypeId, 
                    Description, Balance) values (@Name, @AccountTypeId, @Description, @Balance);
                    SELECT SCOPE_IDENTITY();", account);
            account.Id = id;
        }

        public async Task<IEnumerable<Account>> Search(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            var account = await connection.QueryAsync<Account>(@"SELECT Accounts.Id, Accounts.Name, Accounts.Balance,
                tc.Name AS AccountType FROM Accounts INNER JOIN ACCountTypes tc 
                on Accounts.AccountTypeId = ac.Id WHERE tc.UserId = @UserId
                ORDER BY TC.Order", new { userId });
            return account;
        }

        public async Task<Account> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Account>(@"
                SELECT Accounts.Id, Accounts.Name, Accounts.Balance, Description
                tc.AccountTypeId FROM Accounts INNER JOIN ACCountTypes tc 
                on Accounts.AccountTypeId = ac.Id 
                WHERE tc.UserId = @UserId AND Accounts.Id = @Id", new { id, userId});
        }

        public async Task Update(AccountCreationViewModel account)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@" UPDATE Accounts
                SET Name = @name, Balance = @balance, Description = @description,
                AccountType = @accountType
                WHERE Id = @Id", account);
        }
    }
}
