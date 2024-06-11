using AutoMapper;
using plusminus.Data;
using plusminus.Dtos.Users;
using plusminus.Models;
using Microsoft.EntityFrameworkCore;
using plusminus.Services.CategoryExpansesService;
using plusminus.Services.CategoryIncomesService;

namespace plusminus.Services.UsersService
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly ICategoryExpansesService  _categoryExpansesService;
        private readonly ICategoryIncomesService  _categoryIncomesService;

        public UsersService(IMapper mapper, DataContext context,ICategoryExpansesService categoryExpansesService, ICategoryIncomesService categoryIncomesService)
        {
            _mapper = mapper;
            _context = context;
            _categoryExpansesService = categoryExpansesService;
            _categoryIncomesService = categoryIncomesService;
        }

        public async Task<ServiceResponse<UsersAuthenticateResponse>> Authenticate(UsersAuthenticateRequest user)
        {
            var serviceResponse = new ServiceResponse<UsersAuthenticateResponse>();
            try
            {
                var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);

                if (dbUser == null) throw new Exception("Введен неправильный логин или пароль");
                if (!BCrypt.Net.BCrypt.Verify(user.Password, dbUser.PasswordHash)) throw new Exception("Введен неправильный логин или пароль");
                

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
                var sameLoginUser =  await _context.Users.Where(u => u.Login == user.Login).ToListAsync();
                if (sameLoginUser.Count > 0) throw new Exception("Пользователь с таким логином уже существует");
                
                var sameEmailUser =  await _context.Users.Where(u => u.Email == user.Email).ToListAsync();
                if (sameEmailUser.Count > 0) throw new Exception("Пользователь с такой почтой уже существует");
                
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

                if (user.BaseCategories)
                {
                    await _categoryExpansesService.AddBaseCategories(dbUserEntity.Id);
                    await _categoryIncomesService.AddBaseCategories(dbUserEntity.Id);
                }

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
