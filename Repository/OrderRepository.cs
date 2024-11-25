using Microsoft.EntityFrameworkCore;
using PetShopApi.Data;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Interfaces;

namespace shoppetApi.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ApiDbContext _context;
        public OrderRepository(ApiDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> PetAlreadyExists(int id)
        {
            var data = await _context.Orders
                .Where(x => x.PetId == id)
                .LastOrDefaultAsync();
            if (data != null) {
                if (data.OrderStatus == OrderStatusEnum.Cancelled.ToString() || 
                    data.OrderStatus == OrderStatusEnum.Failed.ToString())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
