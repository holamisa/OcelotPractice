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

        public async Task<UserModel?> GetUserById(int id, bool throwNotFound = false)
        {
            using (var unitOfWork = new UnitOfWork(_context))
            {
                UserModel? user = await unitOfWork.User.GetByIdAsync(id);
                if (throwNotFound && user is null)
                {
                    throw new NotFoundException($"ID : {id} 사용자는 없습니다.");
                }

                return user;
            }
        }

        public async Task<UserModel?> GetUserByEmail(string email, bool throwNotFound = false)
        {
            using (var unitOfWork = new UnitOfWork(_context))
            {
                UserModel? user = await unitOfWork.User.GetByEmailAsync(email);
                if (throwNotFound && user is null)
                {
                    throw new NotFoundException($"Email : {email} 사용자는 없습니다.");
                }

                return user;
            }
        }

        public async Task<int> AddUser(UserModel user)
        {
            UserModel? existingUser = await GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                throw new ConflictException("이미 존재하는 email입니다.");
            }

            using (var unitOfWork = new UnitOfWork(_context, useTransaction: true))
            {
                try
                {
                    int userId = await unitOfWork.User.AddAsync(user);
                    if (userId == 0)
                    {
                        throw new InsertFailedException("사용자 생성 실패했습니다.");
                    }

                    //var newCart = new CartModel { UserId = userId };
                    //await unitOfWork.Cart.AddAsync(newCart);

                    return await unitOfWork.CommitAsync();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<int> UpdateUserById(UpdateUserModel user)
        {
            if (user.Id == 0)
            {
                throw new BadRequestException("옳바른 ID가 아닙니다.");
            }

            using (var unitOfWork = new UnitOfWork(_context, useTransaction: true))
            {
                try
                {
                    await unitOfWork.User.UpdateByIdAsync(user);

                    return await unitOfWork.CommitAsync();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<int> DeleteUserById(int id)
        {
            if (id == 0)
            {
                throw new BadRequestException("옳바른 ID가 아닙니다.");
            }

            using (var unitOfWork = new UnitOfWork(_context, useTransaction: true))
            {
                try
                {
                    await unitOfWork.User.DeleteByIdAsync(id);

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
