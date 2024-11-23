using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Services;

namespace shoppetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreedController : GenericController<Breed, BreedDTO, BreedDTO>
    {
        private readonly IBreedService _breedService;
        private readonly IGenericService<Breed> _genericService;
        public BreedController(IGenericService<Breed> genericService, IMapper mapper, IBreedService breedService) : base(genericService, mapper)
        {
            _breedService = breedService;
            _genericService = genericService;
        }

        [HttpPost("Create")]
        public override async Task<ActionResult<Breed>> Add([FromBody] BreedDTO breedDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var speciesExists = await _breedService.SpeciesExists(breedDTO.SpeciesId);
                var breedExists = await _breedService.BreedExists(breedDTO.BreedName);
                if (!speciesExists)
                {
                    return Conflict(MessageConstants.NotExistsSpecies);
                }
                if (breedExists)
                {
                    return Conflict(MessageConstants.AlreadyExistsBreed);
                }
                breedDTO.BreedName = _genericService.ApplyTitleCase(breedDTO.BreedName);
                return await base.Add(breedDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
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

        [HttpPut("Update/{id}")]
        public override async Task<ActionResult<Breed>> Update(string id, [FromBody] BreedDTO breedDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var speciesExists = await _breedService.SpeciesExists(breedDTO.SpeciesId);
                var breedExists = await _breedService.BreedExists(breedDTO.BreedName);
                if (!speciesExists)
                {
                    return Conflict(MessageConstants.NotExistsSpecies);
                }
                if (breedExists)
                {
                    return Conflict(MessageConstants.AlreadyExistsBreed);
                }
                return await base.Update(id, breedDTO);
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
                if(id <= 0)
                {
                    return BadRequest(MessageConstants.InvalidId);
                }
                var result = await _breedService.GetSameSpeciesBreeds(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }
    }
}
