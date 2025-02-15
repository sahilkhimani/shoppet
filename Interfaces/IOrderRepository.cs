﻿using PetShopApi.Models;

namespace shoppetApi.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<bool> PetAlreadyExists(int id);
        public Task<IEnumerable<Order>> GetMyOrders(string id);
        public Task<IEnumerable<Order>> GetSellerOrderList(string id);
        public Task<Order> GetOrderToUpdateStatus(string userId, int id);
        public Task<string> GetPetOrderStatus(int id);
        public Task<bool> PetBuyerExists(string id);
        public Task<bool> PetOrderExists(int id);
    }
}
