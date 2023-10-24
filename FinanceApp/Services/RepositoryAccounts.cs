using Dapper;
using FinanceApp.Models;
using Microsoft.Data.SqlClient;

namespace FinanceApp.Services
{
    public interface IRepositoryAccounts
    {
        Task Create(Account account);
        Task<IEnumerable<Account>> Search(int userId);
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
                tc.Name AS AccountType FROM Accounts INNER JOIN ACCountTypes ac 
                on Accounts.AccountTypeId = ac.Id WHERE tc.UserId = @UserId
                ORDER BY TC.Order", new { userId });
            return account;
        }
    }
}
