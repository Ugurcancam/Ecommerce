using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Dtos;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Services
{
    public interface IOrderService : IService<Order>
    {
        Task<IEnumerable<Order>> GetAllWithUsers();
        Task<Order> CreateOrderAsync(string userId,OrderDto orderDto);
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order> GetOrderDetailsAsync(int orderId);
        Task<Order> GetOrderDetailsByUserAsync(string userId, int orderId);

    }
}