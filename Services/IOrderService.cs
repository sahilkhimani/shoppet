using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;

namespace shoppetApi.Services
{
    public interface IOrderService
    {
        public Task<APIResponse<Order>> CreateOrder(AddOrderDTO addOrderDTO);
    }
}
