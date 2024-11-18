using Microsoft.AspNetCore.Mvc;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;

namespace shoppetApi.Services
{
    public interface IUserService
    {
        public Task<APIResponse<User>> LoginUser(UserLoginDTO userLoginDTO);
        public Task<APIResponse<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO);

    }
}
