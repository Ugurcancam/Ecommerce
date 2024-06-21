using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Dtos;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllWithUsers();
        Task<IEnumerable<Order>> GetCargoPendingOrdersWithUser();
        Task<IEnumerable<Order>> GetShippedOrdersWithUser();
        Task<IEnumerable<Order>> GetDeliveredOrdersWithUser();
        Task<IEnumerable<Order>> GetLastFiveOrdersWithUser();
        Task<IEnumerable<CityOrderCountDto>> GetMostOrderedCities();
        Task<IEnumerable<ProductOrderCountDto>> GetMostOrderedProducts();
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order> GetOrderDetailsByUserAsync(string userId, int orderId);
        Task<Order> GetOrderDetailsAsync(int orderId);
        Task<Order> GetByPaymentIdAsync(string paymentId);
    }
}