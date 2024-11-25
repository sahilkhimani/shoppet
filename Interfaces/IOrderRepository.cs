using PetShopApi.Models;

namespace shoppetApi.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<bool> PetAlreadyExists(int id);
    }
}
