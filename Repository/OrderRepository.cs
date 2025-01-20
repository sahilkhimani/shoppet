using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using PetShopApi.Data;
using PetShopApi.Models;
using shoppetApi.Enums;
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

        public async Task<IEnumerable<Order>> GetMyOrders(string id)
        {
            var orderList = await _context.Orders
                .Where(x => x.BuyerId == id)
                .ToListAsync();
            return orderList;
        }

        public async Task<Order> GetOrderToUpdateStatus(string userId, int id)
        {
            var order = await _context.Orders
                .Where(x => _context.Pets
                .Any(y => y.PetId == x.PetId && y.OwnerId == userId && x.OrderId == id))
                .FirstOrDefaultAsync();
            return order;
        }

        public async Task<string> GetPetOrderStatus(int id)
        {
            var status = await _context.Orders
                 .Where(o => o.PetId == id)
                  .OrderByDescending(o => o.OrderDate)
                 .Select(o => o.OrderStatus)
                 .FirstOrDefaultAsync();
            return status;
        }

        public async Task<IEnumerable<Order>> GetSellerOrderList(string id)
        {
            var orderList = await _context.Orders
                .Where(x => _context.Pets
                .Any(y => x.PetId == y.PetId && y.OwnerId == id))
                .ToListAsync();
            return orderList;
        }

        public async Task<bool> PetAlreadyExists(int id)
        {
            var data = await _context.Orders
                 .AsNoTracking()
                 .Where(x => x.PetId == id)
                 .OrderByDescending(o => o.OrderId)
                 .FirstOrDefaultAsync();
            if (data != null)
            {
                var orderStatus = data.OrderStatus.ToLower();
                if (orderStatus == OrderStatusEnum.cancelled.ToString() ||
                    orderStatus == OrderStatusEnum.failed.ToString())
                {
                    return true;
                }
                return false;
            }
            return true;
        }

    }
}