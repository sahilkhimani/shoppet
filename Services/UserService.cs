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
        private readonly JwtTokenService _jwtTokenService;
        public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager , IMapper mapper, JwtTokenService jwtTokenService)
        {

            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<APIResponse<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO)
        {
            const string roleValue = "Admin";
            var user = _mapper.Map<User>(userRegistrationDTO);

            var email = await EmailAlreadyExists(userRegistrationDTO.UserEmail);
            if (!email)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.AlreadyExists(userRegistrationDTO.UserEmail), null);
            }
            var role = await _unitOfWork.Roles.GetRole(userRegistrationDTO.RoleId);
            if (role == null || role.Name == roleValue)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.NotFound("Role"), null);
            }

            var result = await _userManager.CreateAsync(user, userRegistrationDTO.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
                return APIResponse<User>.CreateResponse(true, MessageHelper.Success(typeof(User).Name, "created"), null);
              
            }
            else
            {
                return APIResponse<User>.CreateResponse(false, result.Errors.First().Description, null);
            }
        }
                
        public async Task<APIResponse<User>> LoginUser(UserLoginDTO userLoginDTO)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDTO.UserEmail);
            if (user == null)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.Message("Invalid Credentials"), null);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, userLoginDTO.Password);
            if (!isPasswordValid)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.Message("Invalid Credentials"), null);
            }
            var roles = await _userManager.GetRolesAsync(user);

            if(roles.Count == 0)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.NotFound("Role"), null);

            }

            var token = _jwtTokenService.GenerateJwtToken(user, roles);

            if (token == null)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.ErrorOccurred("while generating token"), null);

            }
            return APIResponse<User>.CreateResponse(true, token, null);


        }

        public async Task<bool> EmailAlreadyExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return false;
            }
            return true;
        }



    }
}