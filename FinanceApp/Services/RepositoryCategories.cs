using Dapper;
using FinanceApp.Models;
using Microsoft.Data.SqlClient;

namespace FinanceApp.Services
{
    public interface IRepositoryCategories
    {
        Task Create(Category category);
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
    }
}
