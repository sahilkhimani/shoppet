using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Services;
using shoppetApi.UnitOfWork;

namespace shoppetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
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
            catch (Exception ex) {
                return StatusCode(500, MessageHelper.ErrorOccured(ex.Message));
            }
           
        }


}
}
