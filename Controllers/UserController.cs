using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Services;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace shoppetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

        public class UserController : GenericController<User, UserRegistrationDTO, UserUpdateDTO>
        {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IGenericService<User> genericService, IMapper mapper, IUserService userService) : base(genericService, mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public override async Task<ActionResult<User>> Add([FromBody] UserRegistrationDTO userRegistrationDto)
        {
         
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _userService.RegisterUser(userRegistrationDto);
                if (result.Success)
                {
                    return Ok(result.Message);
                }
                
                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _userService.LoginUser(userLoginDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("GetAll")]
        public override async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            try
            {
                return await base.GetAll();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [Authorize]
        [HttpGet("GetById/{id}")]
        public override async Task<ActionResult<User>> GetById(string id)
        {
            try
            {
                var result = _userService.ValidUser(id);
                if (!result && !User.IsInRole("Admin"))
                {
                    return Unauthorized(MessageConstants.UnAuthorizedUser);
                }
                return await base.GetById(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [Authorize]
        [HttpDelete("Delete/{id}")]
        public override async Task<ActionResult<User>> Delete(string id)
        {
            var result = _userService.ValidUser(id);
            try
            {
                if (!result && !User.IsInRole("Admin"))
                {
                    return Unauthorized(MessageConstants.UnAuthorizedUser);
                }
                return await base.Delete(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public override async Task<ActionResult<User>> Update(string id, [FromBody] UserUpdateDTO userUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = _userService.ValidUser(id);
                if (!result)
                {
                    return Unauthorized(MessageConstants.UnAuthorizedUser);
                }
                var response = await _userService.UpdateUser(id, userUpdateDTO);
                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }
                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

    }
}