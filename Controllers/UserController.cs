using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Services;
using System.Configuration;
using System.Runtime.CompilerServices;

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
           return await base.GetAll();
        }

        [Authorize]
        [HttpGet("GetById/{id}")]
        public override async Task<ActionResult<User>> GetById(string id)
        {
            return await base.GetById(id);
        }

        [Authorize]
        [HttpDelete("Delete/{id}")]
        public override async Task<ActionResult<User>> Delete(string id)
        {
            return await base.Delete(id);
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public override async Task<ActionResult<User>> Update(string id, [FromBody] UserUpdateDTO userUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await base.Update(id, userUpdateDTO);
        }
    }
}