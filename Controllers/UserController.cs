using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Filters;
using shoppetApi.Helper;
using shoppetApi.Services;

namespace shoppetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : GenericController<User, UserRegistrationDTO, UserUpdateDTO>
    {
        private readonly IUserService _userService;

        public UserController(IGenericService<User, UserRegistrationDTO, UserUpdateDTO> genericService, IUserService userService) : base(genericService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public override async Task<ActionResult<User>> Add([FromBody] UserRegistrationDTO userRegistrationDto)
        {
            try
            {
                var result = await _userService.RegisterUser(userRegistrationDto);
                if (!result.Success) return BadRequest(result.Message);
                return Ok(result.Message);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            try
            {
                var result = await _userService.LoginUser(userLoginDTO);
                if (!result.Success) return BadRequest(result.Message);
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [Authorize(Roles = Roles.Admin)]
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
                var result = await _userService.GetById(id);
                if (!result.Success) return Unauthorized(result.Message);
                return Ok(result);
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
            try
            {
                var result = await _userService.DeleteUser(id);
                if (!result.Success) return BadRequest(result.Message);
                return Ok(result.Message);
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
            try
            {
                var result = await _userService.UpdateUser(id, userUpdateDTO);
                if (!result.Success) return BadRequest(result.Message);
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }
    }
}