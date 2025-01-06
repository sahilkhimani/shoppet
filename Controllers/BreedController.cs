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
    public class BreedController : GenericController<Breed, BreedDTO, BreedDTO>
    {
        private readonly IBreedService _breedService;

        public BreedController(IGenericService<Breed, BreedDTO, BreedDTO> genericService, IBreedService breedService) : base(genericService)
        {
            _breedService = breedService;
        }

        [Authorize]
        [HttpPost("Create")]
        public override async Task<ActionResult<Breed>> Add([FromBody] BreedDTO breedDTO)
        {
            try
            {
                var result = await _breedService.Add(breedDTO);
                if (!result.Success) return BadRequest(result.Message);
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("Delete/{id}")]
        public override async Task<ActionResult<Breed>> Delete(string id)
        {
            try
            {
                return await base.Delete(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public override async Task<ActionResult<Breed>> Update(string id, [FromBody] BreedDTO breedDTO)
        {
            try
            {
                var result = await _breedService.Update(id, breedDTO);
                if (!result.Success) return BadRequest(result.Message);
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpGet("GetBySpeciesId/{id}")]
        public async Task<ActionResult> GetBreedBySpeciesId(int id)
        {
            try
            {
                var result = await _breedService.GetSameSpeciesBreeds(id);
                if (!result.Success) return BadRequest(result.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }
    }
}
