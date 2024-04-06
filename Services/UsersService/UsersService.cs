using AutoMapper;
using plusminus.Data;
using plusminus.Dtos.Users;
using plusminus.Models;
using Microsoft.EntityFrameworkCore;
using plusminus.Helpers;

namespace plusminus.Services.UsersService
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UsersService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<UsersAuthenticateResponse>> Authenticate(UsersAuthenticateRequest user)
        {
            var serviceResponse = new ServiceResponse<UsersAuthenticateResponse>();
            try
            {
                var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);

                if (dbUser == null) throw new Exception("Введен неправильный логин или пароль");
                if (!BCrypt.Net.BCrypt.Verify(user.Password, dbUser.PasswordHash)) throw new Exception("Введен неправильный логин или пароль");

                var (accessToken, refreshToken) = JWTHelper.GenerateJwtTokens(dbUser);

                serviceResponse.Data = new UsersAuthenticateResponse
                {
                    Email = dbUser.Email,
                    Id = dbUser.Id,
                    Login = dbUser.Login,
                    Name = dbUser.Name,
                    Phone = dbUser.Phone,
                    SecondName = dbUser.SecondName
                };

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<UsersAuthenticateResponse>> CheckAuth(int userId)
        {
            var serviceResponse = new ServiceResponse<UsersAuthenticateResponse>();
            try
            {
                var dbUser = await _context.Users.FindAsync(userId);
                if (dbUser == null) throw new Exception("Введен неправильный логин или пароль");

                serviceResponse.Data = new UsersAuthenticateResponse
                {
                    Email = dbUser.Email,
                    Id = dbUser.Id,
                    Login = dbUser.Login,
                    Name = dbUser.Name,
                    Phone = dbUser.Phone,
                    SecondName = dbUser.SecondName
                };
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<UsersRegisterResponse>> Register(UsersRegisterRequest user)
        {
            var serviceResponse = new ServiceResponse<UsersRegisterResponse>();
            try
            {
                var addedUser = new User
                {
                    Email = user.Email,
                    Login = user.Login,
                    Name = user.Name,
                    Phone = user.Phone,
                    SecondName = user.SecondName,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password)
                };
                var dbUser = await _context.Users.AddAsync(addedUser);
                await _context.SaveChangesAsync();

                var dbUserEntity = dbUser.Entity;

                if (dbUserEntity == null) throw new Exception("Пользователь не найден");

                var (accessToken, refreshToken) = JWTHelper.GenerateJwtTokens(dbUserEntity);

                serviceResponse.Data = new UsersRegisterResponse
                {
                    Email = dbUserEntity.Email,
                    Id = dbUserEntity.Id,
                    Login = dbUserEntity.Login,
                    Name = dbUserEntity.Name,
                    Phone = dbUserEntity.Phone,
                    SecondName = dbUserEntity.SecondName,
                };


            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}
