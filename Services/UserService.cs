using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Interfaces;
using shoppetApi.MyUnitOfWork;

namespace shoppetApi.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IRoleRepository _roleRepository;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IHttpContextHelper _contextHelper;
        public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper, JwtTokenService jwtTokenService, IHttpContextHelper contextHelper)
        {

            _mapper = mapper;
            _userManager = userManager;
            _roleRepository = unitOfWork.Roles;
            _jwtTokenService = jwtTokenService;
            _contextHelper = contextHelper;
        }

        public async Task<APIResponse<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO)
        {
            var emailExist = await _userManager.FindByEmailAsync(userRegistrationDTO.UserEmail);
            if (emailExist != null) return APIResponse<User>.CreateResponse(false, MessageHelper.AlreadyExists(userRegistrationDTO.UserEmail), null);

            var role = await _roleRepository.GetRole(userRegistrationDTO.RoleId);
            if (role == null || role.Name == Roles.Admin)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.NotFound(nameof(Role)), null);
            }
            var user = _mapper.Map<User>(userRegistrationDTO);
            var result = await _userManager.CreateAsync(user, userRegistrationDTO.Password);

            if (!result.Succeeded) return APIResponse<User>.CreateResponse(false, result.Errors.First().Description, null);
            await _userManager.AddToRoleAsync(user, role.Name!);
            return APIResponse<User>.CreateResponse(true, MessageHelper.Success(nameof(User), MessageConstants.createdMessage), null);
        }

        public async Task<APIResponse<User>> LoginUser(UserLoginDTO userLoginDTO)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDTO.UserEmail);
            if (user == null) return APIResponse<User>.CreateResponse(false, MessageConstants.InvalidCredentialsMessage, null);

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, userLoginDTO.Password);
            if (!isPasswordValid) return APIResponse<User>.CreateResponse(false, MessageConstants.InvalidCredentialsMessage, null);

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0) return APIResponse<User>.CreateResponse(false, MessageHelper.NotFound(nameof(Role)), null);

            var token = _jwtTokenService.GenerateJwtToken(user, roles);
            if (token == null) return APIResponse<User>.CreateResponse(false, MessageConstants.TokenCreationErrorMessage, null);
            
            return APIResponse<User>.CreateResponse(true, token, null);
        }

        public async Task<APIResponse<User>> UpdateUser(string id, UserUpdateDTO userUpdateDTO)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
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
                return APIResponse<User>.CreateResponse(true, MessageHelper.Success(typeof(User).Name, MessageConstants.updatedMessage), null);
            }
            return APIResponse<User>.CreateResponse(false, result.Errors.First().Description, null);
        }

        public bool ValidUser(string id)
        {
            var userId = _contextHelper.GetCurrentUserId();
            var userRole = _contextHelper.GetCurrentUserRole();
            if(userRole != Roles.Admin)
            {
                if (userId == null || userId != id) return false;
            }
            return true;
        }

        public async Task<APIResponse<User>> GetById(string id)
        {
            var user = ValidUser(id);
            if (!user) return APIResponse<User>.CreateResponse(false, MessageConstants.UnAuthorizedUserMessage, null);
            var result = await _userManager.FindByIdAsync(id);
            if (result == null) return APIResponse<User>.CreateResponse(false, MessageConstants.NoUserFoundMessage, null);
            return APIResponse<User>.CreateResponse(true, MessageHelper.Success(nameof(User), MessageConstants.fetchedMessage), result);
        }
    }
}