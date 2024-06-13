using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Services
{
    public interface IOrderService : IService<Order>
    {
        Task<IEnumerable<Order>> GetAllWithUsers();
        Task<Order> CreateOrderAsync(string userId,string city,string district,string postalCode,string? orderNote,string deliveryAddress,string? billingAddress);
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order> GetOrderDetailsAsync(int orderId);
        Task<Order> GetOrderDetailsByUserAsync(string userId, int orderId);

    }
}