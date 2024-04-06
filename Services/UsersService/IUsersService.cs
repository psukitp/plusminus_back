using plusminus.Dtos.Users;
using plusminus.Models;

namespace plusminus.Services.UsersService
{
    public interface IUsersService
    {
        public Task<ServiceResponse<UsersAuthenticateResponse>> Authenticate(UsersAuthenticateRequest user);
        public Task<ServiceResponse<UsersRegisterResponse>> Register(UsersRegisterRequest user);
        public Task<ServiceResponse<UsersAuthenticateResponse>> CheckAuth(int userId);
    }
}
