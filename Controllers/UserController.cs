using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.UnitOfWork;

namespace shoppetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO)
        {
            if (await _unitOfWork.Users.GetByEmailAsync(userRegistrationDTO.UserEmail) != null)
            {
                return Conflict("Email Already in Use");
            }
            else
            {
                var user = new User
                {
                    UserName = userRegistrationDTO.UserName,
                    UserEmail = userRegistrationDTO.UserEmail,
                    Password = PasswordHelper.HashPassword(userRegistrationDTO.Password),
                    PhoneNo = userRegistrationDTO.PhoneNo,
                    RoleId = userRegistrationDTO.RoleId,
                };

                await _unitOfWork.Users.Add(user);
                await _unitOfWork.SaveAsync();


                var role = _unitOfWork.Roles.GetById(user.RoleId);

                return Ok(new
                {
                    Message = "SignUp Succesfull",
                    user.UserName,
                    user.UserEmail,
                    user.PhoneNo,
                    role?.Result.RoleName,
                });

            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginUser(LoginDTO loginDTO){

            var user = await _unitOfWork.Users.GetByEmailAsync(loginDTO.UserEmail);
            if(user == null)
            {
                return Unauthorized("Invalid Credentials");
            }
            else
            {
               if(!PasswordHelper.VerifyPassword(loginDTO.Password, user.Password))
                {
                    return Unauthorized("Invalid Credentials");
                }
                else
                {
                    return Ok(new
                    {
                       Message = "Login Successfull",
                       user.UserEmail,
                       user.UserName,
                       user.PhoneNo
                    });
                }
            }

            
        }


}
}
