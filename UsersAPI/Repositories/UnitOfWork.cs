using System.Data;
using UsersAPI.Infrastructures;
using UsersAPI.Repositories.Cart;
using UsersAPI.Repositories.User;

namespace UsersAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private IDbTransaction? _transaction;
        private bool _disposed;

        public UnitOfWork(DapperContext context, bool useTransaction = true)
        {
            _connection = context.CreateConnection();
            _connection.Open();

            if (useTransaction)
            {
                _transaction = _connection.BeginTransaction();

                if (_transaction == null)
                {
                    throw new InvalidOperationException("No transaction available to commit.");
                }
            }

            User = new UserRepository(_connection, _transaction);
            Cart = new CartRepository(_connection, _transaction);
        }

        public IUserRepository User { get; private set; }
        public ICartRepository Cart { get; private set; }

        public bool HasTransaction => _transaction != null;

        public async Task<int> CommitAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction available to commit.");
            }

            try
            {
                _transaction.Commit();
                return await Task.FromResult(1);
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        public void Rollback()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction available to rollback.");
            }

            _transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
                _disposed = true;
            }
        }
    }
}
