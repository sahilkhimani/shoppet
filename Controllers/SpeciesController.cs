using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Services;
using System.Globalization;

namespace shoppetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeciesController : GenericController<Species, SpeciesDTO, SpeciesDTO>
    {
        private readonly ISpeciesService _speciesService;
        private readonly IGenericService<Species> _genericService;
        public SpeciesController(IGenericService<Species> genericService, IMapper mapper, ISpeciesService speciesService) : base(mapper, genericService)
        {
            _speciesService = speciesService;
            _genericService = genericService;
        }


        [HttpPost("Create")]
        public override async Task<ActionResult<Species>> Add([FromBody] SpeciesDTO speciesDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _speciesService.AlreadyExists(speciesDTO.SpeciesName);
                if (result)
                {
                    return Conflict(MessageConstants.AlredyExistsSpecies);
                }
                speciesDTO.SpeciesName =  _genericService.ApplyTitleCase(speciesDTO.SpeciesName);
                return await base.Add(speciesDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [Authorize(Roles ="Admin")]
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

        [HttpPut("Update/{id}")]
        public override async Task<ActionResult<Species>> Update(string id, [FromBody] SpeciesDTO speciesDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _speciesService.AlreadyExists(speciesDTO.SpeciesName);
                if (result)
                {
                    return Conflict(MessageConstants.AlredyExistsSpecies);
                }
                return await base.Update(id, speciesDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }

        }
    }
}
