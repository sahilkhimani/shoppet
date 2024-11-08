using Microsoft.AspNetCore.Mvc;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.UnitOfWork;

namespace shoppetApi.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericService<User> _genericService;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IGenericService<User> genericService, IUnitOfWork unitOfWork) {
            _genericService = genericService;
            _unitOfWork = unitOfWork;
        }
        public async Task<APIResponse<User>> LoginUser(UserLoginDTO loginDTO)
        {
            
            throw new NotImplementedException();
        }

        public async Task<APIResponse<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO)
        {
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(userRegistrationDTO.UserEmail);
            if (existingUser != null) {
                return new APIResponse<User>
                {
                    Success = false,
                    Message = MessageHelper.AlredyExists(userRegistrationDTO.UserEmail),
                };
            }
            var user = new User
            {
                UserName = userRegistrationDTO.UserName,
                UserEmail = userRegistrationDTO.UserEmail,
                Password = PasswordHelper.HashPassword(userRegistrationDTO.Password),
                PhoneNo = userRegistrationDTO.PhoneNo,
                RoleId = userRegistrationDTO.RoleId,
            };
            return await _genericService.Add(user);
        }

        public async Task<APIResponse<User>> DeleteUser(int id)
        {
           var result = await _genericService.Delete(id);
            return result;
        }

        public async Task<APIResponse<User>> UpdateUser(int id, UserUpdateDTO userUpdateDTO)
        {
            var entity = await _genericService.GetById(id);
            if (entity.Data != null) {
                entity.Data.UserName = userUpdateDTO.UserName;
                entity.Data.Password = PasswordHelper.HashPassword(userUpdateDTO.Password);
                entity.Data.PhoneNo = userUpdateDTO.PhoneNo;
                var result = await _genericService.Update(id, entity.Data);
               
                return result;
            }
            return null;
        }
    }
}
