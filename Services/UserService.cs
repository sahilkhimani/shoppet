using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.MyUnitOfWork;
using shoppetApi.Repository;

namespace shoppetApi.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager ,IMapper mapper)
        {

            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO)
        {
            var user = _mapper.Map<User>(userRegistrationDTO);
            var result = await _userManager.CreateAsync(user, userRegistrationDTO.Password);

            if (result.Succeeded) {
                var role = await _unitOfWork.Roles.GetRole(userRegistrationDTO.RoleId);
                if (role != null) {
                   await _userManager.AddToRoleAsync(user, role.Name);
                    return APIResponse<User>.CreateResponse(true, MessageHelper.Success(typeof(User).Name, "created"), null);
                }
                return APIResponse<User>.CreateResponse(false, MessageHelper.Failure(typeof(User).Name, "created"), null);
            }
            else
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.Failure(typeof(User).Name, "creating"), null);
            }
        }
                
        public async Task<APIResponse<User>> LoginUser(UserLoginDTO userLoginDTO)
        {
            throw new NotImplementedException();
        }



       
    }
}