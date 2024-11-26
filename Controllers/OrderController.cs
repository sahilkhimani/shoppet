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

        [Authorize(Roles="Buyer, Admin")]
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
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [Authorize(Roles ="Buyer, Admin")]
        [HttpGet("GetMyOrders")]
        public override async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            try
            {
                var result = await _orderService.GetMyOrders();
                if (!result.Success)
                {
                    return NotFound(result.Message);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpGet("GetById/{id}")]
        public override async Task<ActionResult<Order>> GetById(string id)
        {
            try
            {
                var result = await _orderService.GetOrderById(id);
                if (!result.Success) return NotFound(result.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [HttpPut("CancelOrder/{id}")]
        public override async Task<ActionResult<Order>> Update(string id, [FromBody] UpdateOrderStatusDTO updateOrderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _orderService.CancelOrder(id, updateOrderDTO);
                if (!result.Success) return NotFound(result.Message);

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public override async Task<ActionResult<Order>> Delete(string id)
        {
            return await base.Delete(id);
        }

        [Authorize(Roles ="Seller, Admin")]
        [HttpGet("GetSellerOrders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetSellerOrder()
        {
            try
            {
                var result = await _orderService.GetSellerOrderList();
                if (!result.Success)
                {
                    return NotFound(result.Message);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

        [Authorize(Roles = "Seller, Admin")]
        [HttpPut("UpdateOrderStatus/{id}")]
        public async Task<ActionResult<Order>> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDTO updateOrderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _orderService.UpdateOrderStatus(id, updateOrderDTO);
                if (!result.Success) return NotFound(result.Message);

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageHelper.ErrorOccurred(ex.Message));
            }
        }

    }
}
