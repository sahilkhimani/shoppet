using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;

namespace shoppetApi.Services
{
    public interface IUserService
    {
        public Task<APIResponse<User>> LoginUser(UserLoginDTO userLoginDTO);
        public Task<APIResponse<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO);
        public Task<APIResponse<User>> UpdateUser(string id, UserUpdateDTO userUpdateDTO);
        public Task<APIResponse<User>> GetById(string id);
        public Task<APIResponse<User>> DeleteUser(string id);
    }
}
