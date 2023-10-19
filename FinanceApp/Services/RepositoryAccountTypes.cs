using Dapper;
using FinanceApp.Models;
using Microsoft.Data.SqlClient;

namespace FinanceApp.Services
{
    public interface IRepositoryAccountTypes
    {
        Task Create(AccountType accountType);
        Task<bool> Exists(string name, int userId);
        Task<IEnumerable<AccountType>> Get(int userId);
    }
    public class RepositoryAccountTypes : IRepositoryAccountTypes
    {
        private readonly string connectionString;
        public RepositoryAccountTypes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task Create(AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO AccountType(Name, UserId, Order) 
                                                    VALUES (@Name, @UserId, 0);
                                                    SELECT SCOPE_IDENTITY();", accountType);
            accountType.Id = id;
        }

        public async Task<bool> Exists(string name, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                @"SELECT 1 FROM AccountType WHERE Name =@name AND UserId = @userId", 
                new { name, userId});
            return exist == 1;
        }

        public async Task<IEnumerable<AccountType>> Get(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountType>(
                @"SELECT Id, Name, Order, FROM AccountType WHERE UserId = @userId",
                new { userId });
        }
    }
}
