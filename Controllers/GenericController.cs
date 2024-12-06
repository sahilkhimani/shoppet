using Microsoft.AspNetCore.Mvc;
using shoppetApi.Filters;
using shoppetApi.Helper;
using shoppetApi.Services;

namespace shoppetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<T, TAdd, TUpdate> : ControllerBase, IGenericController<T, TAdd, TUpdate> where T : class where TAdd : class where TUpdate : class
    {
        private readonly IGenericService<T, TAdd, TUpdate> _genericService;

        public GenericController(IGenericService<T, TAdd, TUpdate> genericService)
        {
            _genericService = genericService;
        }

        [ValidateModelState]
        [HttpPost("Create")]
        public virtual async Task<ActionResult<T>> Add([FromBody] TAdd dto)
        {
            try
            {
                var result = await _genericService.Add(dto);
                if (!result.Success) return BadRequest(result.Message);
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpDelete("Delete/{id}")]
        public virtual async Task<ActionResult<T>> Delete(string id)
        {
            try
            {
                var result = await _genericService.Delete(id);
                if (!result.Success) return BadRequest(result.Message);
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpGet("GetAll")]
        public virtual async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            try
            {
                var result = await _genericService.GetAll();
                if (!result.Success) return BadRequest(result.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpGet("GetById/{id}")]
        public virtual async Task<ActionResult<T>> GetById(string id)
        {
            try
            {
                var result = await _genericService.GetById(id);
                if (!result.Success) return BadRequest(result.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [ValidateModelState]
        [HttpPut("Update/{id}")]
        public virtual async Task<ActionResult<T>> Update(string id, [FromBody] TUpdate dto)
        {
            try
            { 
                var result = await _genericService.Update(id, dto);
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