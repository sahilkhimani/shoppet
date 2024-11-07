using Microsoft.AspNetCore.Mvc;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;

namespace shoppetApi.Services
{
    public interface IUserService
    {
        public Task<APIResponse<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO);
        public Task<APIResponse<User>> LoginUser(LoginDTO loginDTO);
    }
}
