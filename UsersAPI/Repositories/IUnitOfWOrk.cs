using UsersAPI.Repositories.Cart;
using UsersAPI.Repositories.User;

namespace UsersAPI.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        ICartRepository Cart { get; }

        Task<int> CommitAsync();
        void Rollback();
        bool HasTransaction { get; }
    }
}
