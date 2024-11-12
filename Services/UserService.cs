using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserService(IGenericService<User> genericService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _genericService = genericService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponse<User>> LoginUser(UserLoginDTO loginDTO)
        {
            

            throw new NotImplementedException();
        }

        public async Task<APIResponse<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO)
        {
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(userRegistrationDTO.UserEmail);
            if (existingUser != null)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.AlredyExists(userRegistrationDTO.UserEmail), null);
            }
            var user = _mapper.Map<User>(userRegistrationDTO);
            return await _genericService.Add(user);
        }

        public async Task<APIResponse<User>> UpdateUser(int id, UserUpdateDTO userUpdateDTO)
        {
            var entity = await _genericService.GetById(id);
            if (entity.Data == null)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.NotFound("The User is"), null);
            }
            
            entity.Data.UserName = userUpdateDTO.UserName;
            entity.Data.Password = PasswordHelper.HashPassword(userUpdateDTO.Password);
            entity.Data.PhoneNo = userUpdateDTO.PhoneNo;
            return await _genericService.Update(id, entity.Data);
        }
    }
}