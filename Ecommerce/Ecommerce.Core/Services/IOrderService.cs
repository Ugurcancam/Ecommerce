using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string userId);
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order> GetOrderDetailsAsync(string userId, int orderId);
    }
}