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
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register([FromBody] UserRegistrationDTO userRegistrationDto)
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
                return StatusCode(500, MessageHelper.ErrorOccured(ex.Message));
            }
        }


        //[HttpPost("Register")]
        //public async Task<ActionResult<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    try
        //    {
        //        var result = await _userService.RegisterUser(userRegistrationDTO);
        //        if (!result.Success)
        //        {
        //            return Conflict(result.Message);
        //        }

        //        return Ok(result.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, MessageHelper.ErrorOccured(ex.Message));
        //    }
        //}


    }
    }