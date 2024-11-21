using Microsoft.AspNetCore.Mvc;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using System.Security.Claims;

namespace shoppetApi.Services
{
    public interface IUserService
    {
        public Task<APIResponse<User>> LoginUser(UserLoginDTO userLoginDTO);
        public Task<APIResponse<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO);
        public Task<APIResponse<User>> UpdateUser(string id, UserUpdateDTO userUpdateDTO);
        public Task<bool> EmailAlreadyExists(string email);
        public bool ValidUser(string id);
    }
}
