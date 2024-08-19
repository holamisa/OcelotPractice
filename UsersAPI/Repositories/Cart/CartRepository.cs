using Dapper;
using System.Data;
using UsersAPI.Models;

namespace UsersAPI.Repositories.Cart
{
    public class CartRepository : ICartRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction? _transaction;

        public CartRepository(IDbConnection connection, IDbTransaction? transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public async Task<CartModel?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM TBL_CART WHERE Id = @Id";

            return await _connection.QueryFirstOrDefaultAsync(sql);
        }

        public async Task<int> AddAsync(CartModel cart)
        {
            var sql = "INSERT INTO TBL_CART (user_id) VALUES (@UserId)";

            return await _connection.ExecuteAsync(sql, cart, _transaction);
        }

        public Task<int> UpdateByIdAsync(CartModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
