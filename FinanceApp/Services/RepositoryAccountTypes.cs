using Dapper;
using FinanceApp.Models;
using Microsoft.Data.SqlClient;

namespace FinanceApp.Services
{
    public interface IRepositoryAccountTypes
    {
        Task Create(AccountType accountType);
        Task Delete(int id);
        Task<bool> Exists(string name, int userId);
        Task<IEnumerable<AccountType>> Get(int userId);
        Task<AccountType> GetById(int id, int userId);
        Task Sort(IEnumerable<AccountType> sortedAccountType);
        Task Update(AccountType accountType);
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
            var id = await connection.QuerySingleAsync<int>($"AccountTypes_Insert", 
                new { userId = accountType.UserId,
                name = accountType.Name}, commandType: System.Data.CommandType.StoredProcedure);
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
                @"SELECT Id, Name, Order, FROM AccountType WHERE UserId = @userId ORDER BY Order",
                new { userId });
        }

        public async Task Update(AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE AccountType SET Name = @Name WHERE Id = @Id", accountType);
        }

        public async Task<AccountType> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<AccountType>(@"SELECT Id, Name, Order
                    FROM AccountType WHERE Id = @Id AND UserId = @UserId", new { id, userId });
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE FROM AccountType WHERE Id = @Id", id);
        }

        public async Task Sort(IEnumerable<AccountType> sortedAccountType)
        {
            var query = "UPDATE AccountType SET Order = @Order WHERE Id = @Id;";
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, sortedAccountType);

        }
    }
}
