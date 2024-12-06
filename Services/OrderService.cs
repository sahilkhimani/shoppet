using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Enums;
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
        private readonly string userId;


        public OrderService(IUnitOfWork unitOfWork, IHttpContextHelper httpContextHelper)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = _unitOfWork.Orders;
            _contextHelper = httpContextHelper;
            //userId = _contextHelper.GetCurrentUserId();
        }
        public async Task<APIResponse<Order>> CreateOrder(AddOrderDTO addOrderDTO)
        {
            try
            {
                if(addOrderDTO.PetId <= 0)
                {
                    return APIResponse<Order>.CreateResponse(false, MessageConstants.InvalidId, null);
                }
                var petAlreadyOrdered = await _orderRepository.PetAlreadyExists(addOrderDTO.PetId);
                if (!petAlreadyOrdered)
                {
                    return APIResponse<Order>.CreateResponse(false, MessageConstants.PetNotExists, null);
                }
                var petData = await _unitOfWork.Pets.GetById(addOrderDTO.PetId);
                if (petData == null)
                {
                    return APIResponse<Order>.CreateResponse(false, MessageConstants.DataNotFound, null);
                }
                var newOrder = new Order
                {
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    TotalPrice = petData.PetPrice,
                    OrderStatus = OrderStatusEnum.Pending.ToString(),
                    BuyerId = userId,
                    PetId = addOrderDTO.PetId,
                };
                await _orderRepository.Add(newOrder);
                await _unitOfWork.SaveAsync();
                return APIResponse<Order>.CreateResponse(true, MessageHelper.Success(typeof(Order).Name, MessageConstants.createdMessage), newOrder);

            }
            catch (Exception ex)
            {
                return APIResponse<Order>.CreateResponse(false, MessageHelper.Exception(typeof(Order).Name, MessageConstants.creatingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<IEnumerable<Order>>> GetMyOrders()
        {
            try
            {
                var result = await _orderRepository.GetMyOrders(userId);
                if(result == null || !result.Any())
                {
                    return APIResponse<IEnumerable<Order>>.CreateResponse(false, MessageConstants.NoOrderFoundMessage, null);
                }
                return APIResponse<IEnumerable<Order>>.CreateResponse(true, MessageHelper.Success(typeof(Order).Name, MessageConstants.fetchedMessage), result);
            }
            catch (Exception ex)
            {
                return APIResponse<IEnumerable<Order>>.CreateResponse(false, MessageHelper.Exception(typeof(Order).Name, MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<Order>> GetOrderById(string id)
        {
            try {
                object parsedId = id;
                if (int.TryParse(id, out var intId))
                {
                    if (intId <= 0)
                    {
                        return APIResponse<Order>.CreateResponse(false, MessageConstants.InvalidId, null);
                    }
                    parsedId = intId;
                }
                var result = await _unitOfWork.Orders.GetById(parsedId);
                if(result == null)
                {
                    return APIResponse<Order>.CreateResponse(false, MessageConstants.DataNotFound, null);
                }
                if (result.BuyerId == userId)
                {
                    return APIResponse<Order>.CreateResponse(true, MessageHelper.Success(typeof(Order).Name, MessageConstants.fetchedMessage), result);
                }
                return APIResponse<Order>.CreateResponse(false, MessageConstants.UnAuthorizedUserMessage, null);
            }
            catch (Exception ex)
            {
                return APIResponse<Order>.CreateResponse(false, MessageHelper.Exception(typeof(Order).Name, MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<IEnumerable<Order>>> GetSellerOrderList()
        {
            try
            {
                var result = await _orderRepository.GetSellerOrderList(userId);
                if (result == null || !result.Any())
                {
                    return APIResponse<IEnumerable<Order>>.CreateResponse(false, MessageConstants.NoOrderFoundMessage, null);
                }
                return APIResponse<IEnumerable<Order>>.CreateResponse(true, MessageHelper.Success(typeof(Order).Name, MessageConstants.fetchedMessage), result);

            }
            catch (Exception ex)
            {
                return APIResponse<IEnumerable<Order>>.CreateResponse(false, MessageHelper.Exception(typeof(Order).Name, MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<Order>> CancelOrder(string id, UpdateOrderStatusDTO updateOrderDTO)
        {
            try
            {
                object parsedId = id;
                if (int.TryParse(id, out var intId))
                {
                    if (intId <= 0)
                    {
                        return APIResponse<Order>.CreateResponse(false, MessageConstants.InvalidId, null);
                    }
                    parsedId = intId;
                }
                var data = await _orderRepository.GetById(parsedId);
              
                if (data == null)
                {
                    return APIResponse<Order>.CreateResponse(false, MessageConstants.DataNotFound, null);
                }
                var orderStatus = data.OrderStatus;
                var updatedStatus = updateOrderDTO.OrderStatus;
                if (data.BuyerId != userId)
                {
                    return APIResponse<Order>.CreateResponse(false, MessageConstants.UnAuthorizedUserMessage, null);
                }
                if(orderStatus == OrderStatusEnum.Cancelled.ToString())
                {
                    return APIResponse<Order>.CreateResponse(false, MessageConstants.OrderAlreadyCancelledMessage, null);
                }
                if (updatedStatus == OrderStatusEnum.Cancelled.ToString()
                    && orderStatus != OrderStatusEnum.Shipped.ToString()
                    && orderStatus != OrderStatusEnum.Failed.ToString()
                    && orderStatus != OrderStatusEnum.Delivered.ToString())
                {
                    data.OrderStatus = updatedStatus;
                    await _orderRepository.Update(parsedId, data);
                    await _unitOfWork.SaveAsync();
                    return APIResponse<Order>.CreateResponse(true, MessageHelper.Success(typeof(Order).Name, MessageConstants.updatedMessage), null);
                }
                return APIResponse<Order>.CreateResponse(false, MessageConstants.InvalidOperationMessage, null);
            }
            catch (Exception ex)
            {
                return APIResponse<Order>.CreateResponse(false, MessageHelper.Exception(typeof(Order).Name, MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<Order>> UpdateOrderStatus(int id, UpdateOrderStatusDTO updateOrderDTO)
        {
            try
            {
                if (id <= 0)
                {
                    return APIResponse<Order>.CreateResponse(false, MessageConstants.InvalidId, null);
                }
                var data = await _orderRepository.GetOrderToUpdateStatus(userId, id);
                
                if (data == null)
                {
                    return APIResponse<Order>.CreateResponse(false, MessageConstants.DataNotFound, null);
                }
                var orderStatus = data.OrderStatus;
                var updatedStatus = updateOrderDTO.OrderStatus;
                if (orderStatus == OrderStatusEnum.Cancelled.ToString() 
                    || orderStatus == OrderStatusEnum.Failed.ToString()
                    || orderStatus == OrderStatusEnum.Delivered.ToString())
                {
                    return APIResponse<Order>.CreateResponse(false, MessageHelper.AlreadyStatusUpdated(data.OrderStatus), null);
                }
                if ((orderStatus == OrderStatusEnum.Pending.ToString()
                    && updatedStatus == OrderStatusEnum.Processing.ToString())
                    || (orderStatus == OrderStatusEnum.Processing.ToString()
                    && updatedStatus == OrderStatusEnum.Shipped.ToString())
                    || (orderStatus == OrderStatusEnum.Shipped.ToString()
                    && updatedStatus == OrderStatusEnum.Delivered.ToString())
                    || updatedStatus == OrderStatusEnum.Cancelled.ToString()) 
                {
                    data.OrderStatus = updatedStatus;
                    await _orderRepository.Update(id, data);
                    await _unitOfWork.SaveAsync();
                    return APIResponse<Order>.CreateResponse(true, MessageHelper.Success(typeof(Order).Name, MessageConstants.updatedMessage), null);
                }
                return APIResponse<Order>.CreateResponse(false, MessageConstants.InvalidOperationMessage, null);
            }
            catch (Exception ex)
            {
                return APIResponse<Order>.CreateResponse(false, MessageHelper.Exception(typeof(Order).Name, MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

    }
}
