using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly IPetRepository _petRepository;
        private readonly IOrderRepository _orderRepository;
        const string petExistErrorMessage = "You cannot delete account because user pets exists";
        const string OrderExistErrorMessage = "You cannot delete account because user orders exists";

        public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper, JwtTokenService jwtTokenService, IHttpContextHelper contextHelper)
        {

            _mapper = mapper;
            _userManager = userManager;
            _roleRepository = unitOfWork.Roles;
            _petRepository = unitOfWork.Pets;
            _orderRepository = unitOfWork.Orders;
            _jwtTokenService = jwtTokenService;
            _contextHelper = contextHelper;
        }
        public async Task<APIResponse<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO)
        {
            try
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
            catch (Exception ex)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.Exception(nameof(User), MessageConstants.RegisterUserException, ex.Message), null);
            }
        }

        public async Task<APIResponse<User>> LoginUser(UserLoginDTO userLoginDTO)
        {
            try
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
            catch (Exception ex)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.Exception(nameof(User), MessageConstants.LoginUserException, ex.Message), null);
            }
        }

        public async Task<APIResponse<User>> UpdateUser(string id, UserUpdateDTO userUpdateDTO)
        {
            try
            {
                var user = await GetById(id);
                if (!user.Success || user.Data == null) return user;

                var userDetails = user.Data;
                var passwordUpdate = await _userManager.ChangePasswordAsync(userDetails, userUpdateDTO.CurrentPassword, userUpdateDTO.Password);
                if (!passwordUpdate.Succeeded) return APIResponse<User>.CreateResponse(false, passwordUpdate.Errors.First().Description, null);

                userDetails.UserName = userUpdateDTO.UserName;
                userDetails.PhoneNumber = userUpdateDTO.PhoneNumber;

                var result = await _userManager.UpdateAsync(userDetails);
                if (!result.Succeeded) return APIResponse<User>.CreateResponse(false, result.Errors.First().Description, null);

                return APIResponse<User>.CreateResponse(true, MessageHelper.Success(nameof(User), MessageConstants.updatedMessage), null);
            }
            catch (Exception ex)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.Exception(nameof(User), MessageConstants.updatingMessage, ex.Message), null);
            }
        }
        public async Task<APIResponse<User>> GetById(string id)
        {
            try
            {
                var user = ValidUser(id);
                if (!user) return APIResponse<User>.CreateResponse(false, MessageConstants.UnAuthorizedUserMessage, null);
                var result = await _userManager.FindByIdAsync(id);
                if (result == null) return APIResponse<User>.CreateResponse(false, MessageConstants.NoUserFoundMessage, null);
                return APIResponse<User>.CreateResponse(true, MessageHelper.Success(nameof(User), MessageConstants.fetchedMessage), result);
            }
            catch (Exception ex)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.Exception(nameof(User), MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<User>> DeleteUser(string id)
        {
            try
            {
                var user = await GetById(id);
                if (!user.Success || user.Data == null) return user;
                var role = _contextHelper.GetCurrentUserRole();
                if (role == Roles.Seller || role == Roles.Admin)
                {
                    var petExists = await _petRepository.PetExists(id);
                    if (petExists) return APIResponse<User>.CreateResponse(false, petExistErrorMessage, null);
                }
                if (role == Roles.Buyer || role == Roles.Admin)
                {
                    var OrderExists = await _orderRepository.PetBuyerExists(id);
                    if (OrderExists) return APIResponse<User>.CreateResponse(false, OrderExistErrorMessage, null);
                }
                var result = await _userManager.DeleteAsync(user.Data);
                if (!result.Succeeded) return APIResponse<User>.CreateResponse(false, result.Errors.First().Description, null);
                return APIResponse<User>.CreateResponse(true, MessageHelper.Success(nameof(User), MessageConstants.deletedMessage), null);
            }
            catch (Exception ex)
            {
                return APIResponse<User>.CreateResponse(false, MessageHelper.Exception(nameof(User), MessageConstants.deletingMessage, ex.Message), null);
            }
        }
        public bool ValidUser(string id)
        {
            var userId = _contextHelper.GetCurrentUserId();
            var userRole = _contextHelper.GetCurrentUserRole();
            if (userRole != Roles.Admin)
            {
                if (userId == null || userId != id) return false;
            }
            return true;
        }
    }
}