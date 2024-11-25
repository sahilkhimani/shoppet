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

        public PetController(IMapper mapper, IGenericService<Pet> genericService, IPetService petService) : base(mapper, genericService)
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
                data.PetName = _genericService.ApplyTitleCase(data.PetName);

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
                if (ownPet == MessageConstants.InvalidId || ownPet == MessageConstants.DataNotFound)
                {
                    return BadRequest(ownPet);
                }
                if (ownPet == MessageConstants.UnAuthorizedUser) return Unauthorized(ownPet);
                return await base.Delete(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpPut("Update/{id}")]
        public override async Task<ActionResult<Pet>> Update(string id, [FromBody] PetDTO petDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var checkData = await _petService.CheckUser(petDTO);
                if (checkData == MessageConstants.NoUser || checkData == MessageConstants.NotExistsBreed
                    || checkData == MessageConstants.WrongGender)
                {
                    return BadRequest(checkData);
                }
                var ownPet = await _petService.CheckOwnPets(id);
                if (ownPet == MessageConstants.InvalidId || ownPet == MessageConstants.DataNotFound)
                {
                    return BadRequest(ownPet);
                }
                if (ownPet == MessageConstants.UnAuthorizedUser) return Unauthorized(ownPet);
                return await base.Update(id, petDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpGet("GetPetsByBreedId/{id}")]
        public async Task<ActionResult> GetPetsByBreedId(int id)
        {
            try
            {
                var result = await _petService.GetPetsByBreedId(id);
                if (!result.Success) return NotFound(result.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpGet("GetPetsByAge/{age}")]
        public async Task<ActionResult> GetPetsByAge(int age)
        {
            try
            {
                var result = await _petService.GetPetsByAge(age);
                if (!result.Success) return NotFound(result.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpGet("GetPetsByAgeRange")]
        public async Task<ActionResult> GetPetsByAgeRange(int minAge, int maxAge)
        {
            try
            {
                var result = await _petService.GetPetsByAgeRange(minAge, maxAge);
                if (!result.Success) return NotFound(result.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpGet("GetPetsByGender/{gender}")]
        public async Task<ActionResult> GetPetsByGender(string gender)
        {
            try
            {
                var result = await _petService.GetPetsByGender(gender);
                if (!result.Success) return NotFound(result.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpGet("GetYourPets")]
        public async Task<ActionResult> GetYourPets()
        {
            try
            {
                var result = await _petService.GetYourPets();
                if (!result.Success) return NotFound(result.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }
    }
}
