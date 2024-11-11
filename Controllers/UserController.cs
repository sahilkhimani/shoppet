using AutoMapper;
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

    public class UserController : ControllerBase{
        private readonly IUserService _userService;
        private readonly IGenericController<User> _genericController;
        private readonly IMapper _mapper;


        public UserController(IUserService userService, IGenericController<User> genericController, IMapper mapper)
           
        {
            _userService = userService;
            _genericController = genericController;
            _mapper = mapper;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _userService.RegisterUser(userRegistrationDTO);
                if (!result.Success)
                {
                    return Conflict(result.Message);
                }

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, MessageHelper.ErrorOccured(ex.Message));
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            try
            {
                var result = await _genericController.Delete(id);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, MessageHelper.ErrorOccured(ex.Message));
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, UserUpdateDTO userUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if(id <=0) return BadRequest("Id is invalid");
                var result = await _userService.UpdateUser(id, userUpdateDTO);
                return Ok(result.Message);               
            }
            catch (Exception ex)
            {
                return StatusCode(500, MessageHelper.ErrorOccured(ex.Message));
            }
        }
    }
}