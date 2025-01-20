using Microsoft.Identity.Client;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;

namespace shoppetApi.Services
{
    public interface IOrderService
    {
        public Task<APIResponse<Order>> CreateOrder(AddOrderDTO addOrderDTO);
        public Task<APIResponse<IEnumerable<Order>>> GetMyOrders();
        public Task<APIResponse<IEnumerable<Order>>> GetSellerOrderList();
        public Task<APIResponse<Order>> GetOrderById(string id);
        public Task<APIResponse<Order>> CancelOrder(string id, UpdateOrderStatusDTO updateOrderDTO);
        public Task<APIResponse<Order>> UpdateOrderStatus(int id, UpdateOrderStatusDTO updateOrderDTO);
        public Task<APIResponse<string>> GetPetOrderStatus(int id);

    }
}
