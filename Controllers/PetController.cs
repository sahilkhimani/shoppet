using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Filters;
using shoppetApi.Helper;
using shoppetApi.Services;

namespace shoppetApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : GenericController<Pet, PetDTO, PetDTO>
    {
        private readonly IPetService _petService;

        private const string Admin = Roles.Admin;
        private const string Seller = Roles.Seller;

        public PetController(IGenericService<Pet, PetDTO, PetDTO> genericService, IPetService petService) : base(genericService)
        {
            _petService = petService;
        }

        [Authorize(Roles = $"{Admin},{Seller}")]
        [ValidateModelState]
        [HttpPost("Add")]
        public override async Task<ActionResult<Pet>> Add([FromBody] PetDTO petDTO)
        {
            try
            {
                var result = await _petService.Add(petDTO);
                if (!result.Success) return BadRequest(result.Message);
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
                var result = await _petService.Delete(id);
                if (!result.Success) return BadRequest(result.Message);
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [ValidateModelState]
        [HttpPut("Update/{id}")]
        public override async Task<ActionResult<Pet>> Update(string id, [FromBody] PetDTO petDTO)
        {
            try
            {
                var result = await _petService.Update(id, petDTO);
                if (!result.Success) return BadRequest(result.Message);
                return Ok(result.Message);
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
                if (!result.Success) return BadRequest(result.Message);
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
                if (!result.Success) return BadRequest(result.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [Authorize(Roles = $"{Admin},{Seller}")]
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