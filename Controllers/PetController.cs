using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Interfaces;
using shoppetApi.Services;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace shoppetApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : GenericController<Pet, PetDTO, PetDTO>
    {
        private readonly IMapper _mapper;
        private readonly IGenericService<Pet> _genericService;
        private readonly IPetService _petService;

        public PetController(IGenericService<Pet> genericService, IMapper mapper,
            IPetService petService) : base(genericService, mapper)
        {
            _mapper = mapper;
            _genericService = genericService;
            _petService = petService;
        }

        [HttpPost("Add")]
        public override async Task<ActionResult<Pet>> Add([FromBody] PetDTO petDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var checkData = await _petService.CheckUser(petDTO);
                if (checkData == MessageConstants.UnAuthenticatedUser || checkData == MessageConstants.NotExistsBreed
                    || checkData == MessageConstants.WrongGender)
                {
                    return BadRequest(checkData);
                }

                var data = _mapper.Map<Pet>(petDTO);
                data.OwnerId = checkData;
                                
                var result = await _genericService.Add(data);
                if (!result.Success)
                {
                    return Conflict(result.Message);
                }

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpDelete("Delete/{id}")]
        public override async Task<ActionResult<Pet>> Delete(string id)
        {
            try
            {
                var ownPet = await _petService.CheckOwnPets(id);
                if (!ownPet)
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
    }
}
