using Dapper;
using System.Data;
using UsersAPI.Models;

namespace UsersAPI.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction? _transaction;

        public UserRepository(IDbConnection connection, IDbTransaction? transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public async Task<UserModel?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM TBL_USER WHERE id = @Id";

            return await _connection.QueryFirstOrDefaultAsync<UserModel>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(UserModel user)
        {
            var sql = "INSERT INTO TBL_USER (name, email, password) VALUES (@Name, @Email, @Password)";

            return await _connection.ExecuteAsync(sql, user, _transaction);
        }

        public async Task<int> UpdateByIdAsync(UserModel user)
        {
            var sql = "UPDATE TBL_USER ";

            return await _connection.ExecuteAsync(sql, user, _transaction);
        }
    }
}
