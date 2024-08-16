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
            using (var unitOfWork = new UnitOfWork(_context, useTransaction: false))
            {
                UserModel? user = await unitOfWork.User.GetByIdAsync(id);
                if (user is null)
                {
                    throw new NotFoundException($"ID : {id} 사용자는 없습니다.");
                }

                return user;
            }
        }

        public async Task<int> AddUser()
        {
            using (var unitOfWork = new UnitOfWork(_context, useTransaction: true))
            {
                try
                {
                    var newUser = new UserModel { Name = "정봉재", Email = "bongjaejeong@naver.com", Password = "12341234" };
                    await unitOfWork.User.AddAsync(newUser);

                    var newCart = new CartModel { UserId = newUser.Id };
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
