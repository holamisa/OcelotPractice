using Middleware.Exceptions.types;
using UsersAPI.Infrastructures;
using UsersAPI.Models;
using UsersAPI.Repositories;

namespace UsersAPI.Services
{
    public class UserService
    {
        private readonly DapperContext _context;

        public UserService(DapperContext context)
        {
            _context = context;
        }

        public async Task<UserModel?> GetUserById(int id)
        {
            using (var unitOfWork = new UnitOfWork(_context))
            {
                UserModel? user = await unitOfWork.User.GetByIdAsync(id);
                if (user is null)
                {
                    throw new NotFoundException($"ID : {id} 사용자는 없습니다.");
                }

                return user;
            }
        }

        public async Task<int> AddUser(UserModel user)
        {
            using (var unitOfWork = new UnitOfWork(_context, useTransaction: true))
            {
                try
                {
                    await unitOfWork.User.AddAsync(user);

                    var newCart = new CartModel { UserId = user.Id };
                    await unitOfWork.Cart.AddAsync(newCart);

                    return await unitOfWork.CommitAsync();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }
    }
}
