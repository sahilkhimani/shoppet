using AutoMapper;
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
        private readonly IPetRepository _petRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextHelper _contextHelper;
        private readonly string userId;

        public const string PetNotExistsMessage = "Pet Not Exists";
        public const string NoOrderFoundMessage = "No Orders Yet";
        public const string OrderAlreadyCancelledMessage = "Order is already cancelled";
        public const string InvalidOperationMessage = "Invalid Operation Performed";

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextHelper httpContextHelper)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = _unitOfWork.Orders;
            _petRepository = _unitOfWork.Pets;
            _mapper = mapper;
            _contextHelper = httpContextHelper;
            userId = _contextHelper.GetCurrentUserId();
        }
        public async Task<APIResponse<Order>> CreateOrder(AddOrderDTO addOrderDTO)
        {
            try
            {
                if (addOrderDTO.PetId <= 0) return APIResponse<Order>.CreateResponse(false, MessageConstants.InvalidId, null);
                var petAlreadyOrdered = await PetAlreadyExists(addOrderDTO.PetId);
                if (!petAlreadyOrdered) return APIResponse<Order>.CreateResponse(false, PetNotExistsMessage, null);

                var petData = await _petRepository.GetById(addOrderDTO.PetId);
                if (petData == null) return APIResponse<Order>.CreateResponse(false, MessageConstants.DataNotFound, null);

                var data = _mapper.Map<Order>(addOrderDTO);
                data.TotalPrice = petData.PetPrice;
                data.BuyerId = userId;

                await _orderRepository.Add(data);
                await _unitOfWork.SaveAsync();
                return APIResponse<Order>.CreateResponse(true, MessageHelper.Success(nameof(Order), MessageConstants.createdMessage), data);
            }
            catch (Exception ex)
            {
                return APIResponse<Order>.CreateResponse(false, MessageHelper.Exception(nameof(Order), MessageConstants.creatingMessage, ex.Message), null);
            }
        }

        private Task<bool> PetAlreadyExists(int id)
        {
            return _orderRepository.PetAlreadyExists(id);
        }
        public async Task<APIResponse<IEnumerable<Order>>> GetMyOrders()
        {
            try
            {
                var result = await _orderRepository.GetMyOrders(userId);
                if (result == null || !result.Any())
                {
                    return APIResponse<IEnumerable<Order>>.CreateResponse(false, NoOrderFoundMessage, null);
                }
                return APIResponse<IEnumerable<Order>>.CreateResponse(true, MessageHelper.Success(nameof(Order), MessageConstants.fetchedMessage), result);
            }
            catch (Exception ex)
            {
                return APIResponse<IEnumerable<Order>>.CreateResponse(false, MessageHelper.Exception(nameof(Order), MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<Order>> GetOrderById(string id)
        {
            try
            {
                var parsedId = HelperMethods.ParseId(id);
                if (parsedId == null) return APIResponse<Order>.CreateResponse(false, MessageConstants.InvalidId, null);

                var result = await _orderRepository.GetById(parsedId);
                if (result == null) return APIResponse<Order>.CreateResponse(false, MessageConstants.DataNotFound, null);

                var role = _contextHelper.GetCurrentUserRole();
                if (role == Roles.Buyer)
                {
                    if (result.BuyerId != userId)
                    {
                        return APIResponse<Order>.CreateResponse(false, MessageConstants.UnAuthorizedUserMessage, null);
                    }
                    return APIResponse<Order>.CreateResponse(true, MessageHelper.Success(nameof(Order), MessageConstants.fetchedMessage), result);
                }
                if (role == Roles.Seller)
                {
                    if (userId != _petRepository.GetOwnerOnPetId(result.PetId))
                    {
                        return APIResponse<Order>.CreateResponse(false, MessageConstants.UnAuthorizedUserMessage, null);
                    }
                    return APIResponse<Order>.CreateResponse(true, MessageHelper.Success(nameof(Order), MessageConstants.fetchedMessage), result);
                }
                return APIResponse<Order>.CreateResponse(true, MessageHelper.Success(nameof(Order), MessageConstants.fetchedMessage), result);
            }
            catch (Exception ex)
            {
                return APIResponse<Order>.CreateResponse(false, MessageHelper.Exception(nameof(Order), MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<IEnumerable<Order>>> GetSellerOrderList()
        {
            try
            {
                var result = await _orderRepository.GetSellerOrderList(userId);
                if (result == null || !result.Any())
                {
                    return APIResponse<IEnumerable<Order>>.CreateResponse(false, NoOrderFoundMessage, null);
                }
                return APIResponse<IEnumerable<Order>>.CreateResponse(true, MessageHelper.Success(nameof(Order), MessageConstants.fetchedMessage), result);

            }
            catch (Exception ex)
            {
                return APIResponse<IEnumerable<Order>>.CreateResponse(false, MessageHelper.Exception(nameof(Order), MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        private bool CheckStatusForCancel(string updatedStatus, string orderStatus)
        {
            var UpdatedStatus = updatedStatus.ToLower();
            var OrderStatus = orderStatus.ToLower();
            return (UpdatedStatus == OrderStatusEnum.cancelled.ToString()
                     && OrderStatus != OrderStatusEnum.shipped.ToString()
                     && OrderStatus != OrderStatusEnum.failed.ToString()
                     && OrderStatus != OrderStatusEnum.delivered.ToString());
        }

        private bool CheckAlreadyUpdatedStatus(string orderStatus)
        {
            var OrderStatus = orderStatus.ToLower();
            return (OrderStatus == OrderStatusEnum.cancelled.ToString()
                    || OrderStatus == OrderStatusEnum.failed.ToString()
                    || OrderStatus == OrderStatusEnum.delivered.ToString());
        }

        private bool CheckForUpdateOrderStatus(string UpdatedStatus, string OrderStatus)
        {
            var updatedStatus = UpdatedStatus.ToLower();
            var orderStatus = OrderStatus.ToLower();
            return ((orderStatus == OrderStatusEnum.pending.ToString()
                    && updatedStatus == OrderStatusEnum.processing.ToString())
                    || (orderStatus == OrderStatusEnum.processing.ToString()
                    && updatedStatus == OrderStatusEnum.shipped.ToString())
                    || (orderStatus == OrderStatusEnum.shipped.ToString()
                    && updatedStatus == OrderStatusEnum.delivered.ToString())
                    || updatedStatus == OrderStatusEnum.cancelled.ToString()
                    || updatedStatus == OrderStatusEnum.failed.ToString());
        }
        public async Task<APIResponse<Order>> CancelOrder(string id, UpdateOrderStatusDTO updateOrderDTO)
        {
            try
            {
                var data = await GetOrderById(id);
                if (!data.Success || data.Data == null) return data;
                var parsedId = HelperMethods.ParseId(id);

                if (userId != data.Data.BuyerId)
                {
                    return APIResponse<Order>.CreateResponse(false, MessageConstants.UnAuthorizedUserMessage, null);
                }
                var orderStatus = data.Data.OrderStatus.ToLower();
                var updatedStatus = updateOrderDTO.OrderStatus.ToLower();

                if (orderStatus == OrderStatusEnum.cancelled.ToString())
                {
                    return APIResponse<Order>.CreateResponse(false, OrderAlreadyCancelledMessage, null);
                }
                if (CheckStatusForCancel(updatedStatus, orderStatus))
                {
                    data.Data.OrderStatus = HelperMethods.ApplyTitleCase(updatedStatus);
                    await _orderRepository.Update(parsedId!, data.Data);
                    await _unitOfWork.SaveAsync();
                    return APIResponse<Order>.CreateResponse(true, MessageHelper.Success(nameof(Order), MessageConstants.updatedMessage), null);
                }
                return APIResponse<Order>.CreateResponse(false, InvalidOperationMessage, null);
            }
            catch (Exception ex)
            {
                return APIResponse<Order>.CreateResponse(false, MessageHelper.Exception(nameof(Order), MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<Order>> UpdateOrderStatus(int id, UpdateOrderStatusDTO updateOrderDTO)
        {
            try
            {
                if (id <= 0) return APIResponse<Order>.CreateResponse(false, MessageConstants.InvalidId, null);
                var data = await _orderRepository.GetOrderToUpdateStatus(userId, id);
                if (data == null) return APIResponse<Order>.CreateResponse(false, MessageConstants.DataNotFound, null);

                var orderStatus = data.OrderStatus;
                var updatedStatus = updateOrderDTO.OrderStatus;
                if (CheckAlreadyUpdatedStatus(orderStatus))
                {
                    return APIResponse<Order>.CreateResponse(false, MessageHelper.AlreadyStatusUpdated(data.OrderStatus), null);
                }
                if (CheckForUpdateOrderStatus(updatedStatus, orderStatus))
                {
                    data.OrderStatus = HelperMethods.ApplyTitleCase(updatedStatus);
                    await _orderRepository.Update(id, data);
                    await _unitOfWork.SaveAsync();
                    return APIResponse<Order>.CreateResponse(true, MessageHelper.Success(nameof(Order), MessageConstants.updatedMessage), null);
                }
                return APIResponse<Order>.CreateResponse(false, InvalidOperationMessage, null);
            }
            catch (Exception ex)
            {
                return APIResponse<Order>.CreateResponse(false, MessageHelper.Exception(nameof(Order), MessageConstants.fetchingMessage, ex.Message), null);
            }
        }
    }
}