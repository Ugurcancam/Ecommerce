using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;
using Ecommerce.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repository.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }


        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                                        .Where(o => o.UserId == userId)
                                        .Include(o => o.OrderItems)
                                        .ThenInclude(oi => oi.Product)
                                        .ToListAsync();
        }
        public async Task<Order> GetOrderDetailsByUserAsync(string userId, int orderId)
        {
            return await _context.Orders
                                .Where(order => order.UserId == userId && order.Id == orderId)
                                .Include(order => order.User)
                                .Include(order => order.OrderItems)
                                .ThenInclude(item => item.Product)
                                .FirstOrDefaultAsync();
        }
        public async Task<Order> GetOrderDetailsAsync(int orderId)
        {
            return await _context.Orders
                                .Where(order => order.Id == orderId)
                                .Include(order => order.User)
                                .Include(order => order.OrderItems)
                                .ThenInclude(item => item.Product)
                                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetAllWithUsers()
        {
            return await _context.Orders.Include(order => order.User).ToListAsync();
        }
    }
}