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
                                        .Include(o => o.OrderItems)
                                        .ThenInclude(oi => oi.Product)
                                        .Where(o => o.UserId == userId)
                                        .ToListAsync();
        }
    }
}