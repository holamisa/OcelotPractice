using MySql.Data.MySqlClient;
using System.Data;

namespace UsersAPI.Infrastructures
{
    public class DapperContext
    {
        private readonly string _connectionString;
        private readonly IConfiguration? _configuration = null;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;

            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
                new ArgumentException("ConnectionString");

            _connectionString = connectionString!;
        }

        public IDbConnection CreateConnection()
        => new MySqlConnection(_connectionString);

        public IDbTransaction BeginTransaction(IDbConnection connection)
            => connection.BeginTransaction();

    }
}
