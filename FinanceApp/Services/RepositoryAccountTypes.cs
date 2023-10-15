using Dapper;
using FinanceApp.Models;
using Microsoft.Data.SqlClient;

namespace FinanceApp.Services
{
    public interface IRepositoryAccountTypes
    {
        void Create(AccountType accountType);
    }
    public class RepositoryAccountTypes : IRepositoryAccountTypes
    {
        private readonly string connectionString;
        public RepositoryAccountTypes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public void Create(AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            var id = connection.QuerySingle<int>($@"INSERT INTO AccountType(Name, UserId, Order) 
                                                    VALUES (@Name, @UserId, 0);
                                                    SELECT SCOPE_IDENTITY();", accountType);
            accountType.Id = id;
        }
    }
}
