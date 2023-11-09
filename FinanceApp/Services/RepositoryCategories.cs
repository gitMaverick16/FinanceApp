using Dapper;
using FinanceApp.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace FinanceApp.Services
{
    public interface IRepositoryCategories
    {
        Task Create(Category category);
        Task Delete(int id);
        Task<IEnumerable<Category>> Get(int userId);
        Task<Category> GetById(int id, int userId);
        Task Update(Category category);
    }
    public class RepositoryCategories : IRepositoryCategories
    {
        private readonly string connectionString;

        public RepositoryCategories(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task Create(Category category)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"
                INSERT INTO Category (Name, OperationTypeId, UserId)
                Values (@Name, @OperationTypeId, @UserId)
                SELECT SCOPE_IDENTITY();", category);
            category.Id = id;
        }

        public async Task<IEnumerable<Category>> Get(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Category>("SELECT * FROM Category WHERE UserId = @userId",
                new { userId });
        }

        public async Task<Category> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Category>(@"SELECT * FROM Category
                WHERE Id = @Id AND UserId = @userId", new { id, userId});
        }

        public async Task Update(Category category)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Category SET Name = @Name, OperationTypeId = @OperationTypeId
                WHERE Id = @Id",
                category);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE Category WHERE Id = @Id", new { id });
        }
    }
}
