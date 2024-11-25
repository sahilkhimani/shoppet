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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : GenericController<Order, AddOrderDTO, UpdateOrderStatusDTO>
    {
        private readonly IOrderService _orderService;
        public OrderController(IMapper mapper, IGenericService<Order> genericService, IOrderService orderService) : base(mapper, genericService)
        {
            _orderService = orderService;
        }

        [HttpPost("CreateOrder")]
        public override async Task<ActionResult<Order>> Add([FromBody] AddOrderDTO addOrderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _orderService.CreateOrder(addOrderDTO);
                if (!result.Success) {
                    return BadRequest(result.Message);
                }
                return Ok(result);  
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }


    }
}
