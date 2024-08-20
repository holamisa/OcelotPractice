using UsersAPI.Models;

namespace UsersAPI.Repositories.User
{
    public interface IUserRepository : IGenericRepository<UserModel, UpdateUserModel>
    {
        Task<UserModel?> GetByEmailAsync(string email);
    }
}
