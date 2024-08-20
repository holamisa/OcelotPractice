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
            throw new NotImplementedException();
        }

        public async Task<int> AddAsync(CartModel cart)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateByIdAsync(CartModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
