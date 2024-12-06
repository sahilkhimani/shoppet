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
    public class SpeciesController : GenericController<Species, SpeciesDTO, SpeciesDTO>
    {
        private readonly ISpeciesService _speciesService;

        public SpeciesController(IGenericService<Species, SpeciesDTO, SpeciesDTO> genericService, ISpeciesService speciesService) : base(genericService)
        {
            _speciesService = speciesService;
        }

        [ValidateModelState]
        [HttpPost("Create")]
        public override async Task<ActionResult<Species>> Add([FromBody] SpeciesDTO speciesDTO)
        {
            try
            {
                var result = await _speciesService.Add(speciesDTO);
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
        public override async Task<ActionResult<Species>> Delete(string id)
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

        [ValidateModelState]
        [HttpPut("Update/{id}")]
        public override async Task<ActionResult<Species>> Update(string id, [FromBody] SpeciesDTO speciesDTO)
        {
            try
            {
                var result = await _speciesService.Update(id, speciesDTO);
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
