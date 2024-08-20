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
            var sql = @"SELECT *
                        FROM TBL_USER
                        WHERE id = @Id
                        AND deleted_at IS NULL;";

            return await _connection.QueryFirstOrDefaultAsync<UserModel>(sql, new { Id = id });
        }

        public async Task<UserModel?> GetByEmailAsync(string email)
        {
            var sql = @"SELECT *
                        FROM TBL_USER
                        WHERE email = @Email
                        AND deleted_at IS NULL;";

            return await _connection.QueryFirstOrDefaultAsync<UserModel>(sql, new { Email = email });
        }

        public async Task<UserModel?> GetByUserAsync(int id)
        {
            var sql = @"SELECT *
                        FROM TBL_USER
                        WHERE id = @Id
                        AND deleted_at IS NULL;";

            return await _connection.QueryFirstOrDefaultAsync<UserModel>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(UserModel user)
        {
            var sql = @"INSERT INTO TBL_USER 
                        (name, email, password)
                        VALUES
                        (@Name, @Email, @Password);
                        SELECT LAST_INSERT_ID();";

            return await _connection.ExecuteScalarAsync<int>(sql, user, _transaction);
        }

        public async Task<int> UpdateByIdAsync(UpdateUserModel user)
        {
            var sql = @"UPDATE TBL_USER
                        SET name = IF(@Name IS NOT NULL, @Name, name)
                        , email = IF(@Email IS NOT NULL, @Email, email)
                        , password = IF(@Password IS NOT NULL, @Password, password)
                        WHERE id = @Id;";

            return await _connection.ExecuteAsync(sql, user, _transaction);
        }

        public async Task<int> DeleteByIdAsync(int id)
        {
            var sql = @"UPDATE TBL_USER
                        SET deleted_at = NOW()
                        WHERE id = @Id;";

            return await _connection.ExecuteAsync(sql, new {Id = id }, _transaction);
        }
    }
}
