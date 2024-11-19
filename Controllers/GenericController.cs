﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Services;
using System.Runtime.CompilerServices;

namespace shoppetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<T, TAdd, TUpdate> : ControllerBase, IGenericController<T, TAdd, TUpdate> where T : class where TAdd : class where TUpdate : class
    {
        private readonly IGenericService<T> _genericService;
        private readonly IMapper _mapper;
        public GenericController(IGenericService<T> genericService, IMapper mapper)
        {
            _genericService = genericService;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public virtual async Task<ActionResult<T>> Add([FromBody] TAdd dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var data = _mapper.Map<T>(dto);
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
        public virtual async Task<ActionResult<T>> Delete(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Id is invalid");
                var result = await _genericService.Delete(id);
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
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }
        [HttpGet("GetById/{id}")]

        public virtual  async Task<ActionResult<T>> GetById(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Id is invalid");
                var result = await _genericService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }
        [HttpPut("Update{id}")]

        public virtual async Task<ActionResult<T>> Update(int id, [FromBody] TUpdate dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (id <= 0) return BadRequest("Id is invalid");
                var data = await _genericService.GetById(id);
                if (!data.Success) return NotFound(data.Message);

                var updatedData = _mapper.Map(dto, data.Data);
                var updated = _mapper.Map<T>(updatedData);

                var result = await _genericService.Update(id, updated);
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }
    }
}