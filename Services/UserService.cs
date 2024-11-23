using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Interfaces;
using shoppetApi.MyUnitOfWork;
using shoppetApi.Repository;
using System.Security.Claims;

namespace shoppetApi.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IHttpContextHelper _contextHelper;
        public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager , IMapper mapper, JwtTokenService jwtTokenService, IHttpContextHelper contextHelper)
        {

            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
            _contextHelper = contextHelper;
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
            return APIResponse<User>.CreateResponse(false, result.Errors.First().Description, null);
     
        }
                
        public async Task<APIResponse<User>> LoginUser(UserLoginDTO userLoginDTO)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDTO.UserEmail);
            if (user == null)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.NotFound(typeof(User).Name), null);
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

        public async Task<APIResponse<User>> UpdateUser(string id, UserUpdateDTO userUpdateDTO)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.NotFound(typeof(User).Name), null);
            }
            if (!userUpdateDTO.CurrentPassword.IsNullOrEmpty() && !userUpdateDTO.Password.IsNullOrEmpty())
            {
                var passwordUpdate = await _userManager.ChangePasswordAsync(user, userUpdateDTO.CurrentPassword, userUpdateDTO.Password);
                if (!passwordUpdate.Succeeded)
                {
                    return APIResponse<User>.CreateResponse(false, passwordUpdate.Errors.First().Description, null);
                }
            }
            user.UserName = userUpdateDTO.UserName;
            user.PhoneNumber = userUpdateDTO.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return APIResponse<User>.CreateResponse(true, MessageHelper.Success(typeof(User).Name, "Updated"), null);
            }
            return APIResponse<User>.CreateResponse(false, result.Errors.First().Description, null);
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

        public bool ValidUser(string id)
        {
            var userIdClaim = _contextHelper.GetCurrentUserId();
            if (userIdClaim == null || userIdClaim != id)
            {
                return false;
            }
            return true;
        }

    }
}