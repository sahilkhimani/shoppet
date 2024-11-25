using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Interfaces;
using shoppetApi.MyUnitOfWork;

namespace shoppetApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextHelper _contextHelper;


        public OrderService(IUnitOfWork unitOfWork, IHttpContextHelper httpContextHelper)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = _unitOfWork.Orders;
            _contextHelper = httpContextHelper;
        }
        public async Task<APIResponse<Order>> CreateOrder(AddOrderDTO addOrderDTO)
        {
            try
            {
                if(addOrderDTO.PetId <= 0)
                {
                    return APIResponse<Order>.CreateResponse(false, MessageConstants.InvalidId, null);
                }
                var petData = await _unitOfWork.Pets.GetById(addOrderDTO.PetId);
                if (petData == null)
                {
                    return APIResponse<Order>.CreateResponse(false, MessageConstants.DataNotFound, null);
                }
                var petAlreadyOrdered = await _orderRepository.PetAlreadyExists(addOrderDTO.PetId);
                if (!petAlreadyOrdered)
                {
                    return APIResponse<Order>.CreateResponse(false, MessageConstants.PetNotExists, null);
                }
                var newOrder = new Order
                {
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    TotalPrice = petData.PetPrice,
                    OrderStatus = OrderStatusEnum.Pending.ToString(),
                    BuyerId = _contextHelper.GetCurrentUserId(),
                    PetId = addOrderDTO.PetId,
                };
                await _orderRepository.Add(newOrder);
                await _unitOfWork.SaveAsync();
                return APIResponse<Order>.CreateResponse(true, MessageHelper.Success(typeof(Order).Name, "created"), newOrder);

            }
            catch (Exception ex)
            {
                return APIResponse<Order>.CreateResponse(false, MessageHelper.Exception(typeof(Order).Name, "creating", ex.Message), null);
            }

        }
    }
}
