using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Dtos;
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
                                        .AsNoTracking()
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
            return await _context.Orders.Include(order => order.User).AsNoTracking().ToListAsync();
        }

        public async Task<Order> GetByPaymentIdAsync(string paymentId)
        {
            return await _context.Orders.Where(o => o.PaymentId == paymentId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetCargoPendingOrdersWithUser()
        {
            return await _context.Orders.Where(o => o.OrderState == OrderState.CargoPending && o.IsPaid == true).Include(o => o.User).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetShippedOrdersWithUser()
        {
            return await _context.Orders.Where(o => o.OrderState == OrderState.Shipped).Include(o => o.User).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetDeliveredOrdersWithUser()
        {
            return await _context.Orders.Where(o => o.OrderState == OrderState.Delivered).Include(o => o.User).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<CityOrderCountDto>> GetMostOrderedCities()
        {
            return await _context.Orders
                                        .Where(o => o.IsPaid == true)
                                        .GroupBy(o => o.City)
                                        .Select(group => new CityOrderCountDto
                                        {
                                            City = group.Key,
                                            OrderCount = group.Count()
                                        })
                                        .OrderByDescending(x => x.OrderCount)
                                        .AsNoTracking()
                                        .Take(6)
                                        .ToListAsync();
        }

        public async Task<IEnumerable<ProductOrderCountDto>> GetMostOrderedProducts()
        {
            return await _context.OrderItems
                            .Where(oi => oi.Order.IsPaid == true)
                            .GroupBy(oi => oi.Product.Name)
                            .Select(group => new ProductOrderCountDto
                            {
                                ProductName = group.Key,
                                CategoryName = group.FirstOrDefault().Product.Category.Name,
                                OrderCount = group.Sum(oi => oi.Quantity),
                                Price = group.FirstOrDefault().Product.Price,
                                Revenue = group.Sum(oi => oi.Quantity * oi.Product.Price),
                                Status = (bool)group.FirstOrDefault().Product.IsActive,
                                InStock = (bool)group.FirstOrDefault().Product.InStock,
                                Stock = group.FirstOrDefault().Product.Stock
                            })
                            .OrderByDescending(x => x.OrderCount)
                            .Take(6)
                            .AsNoTracking()
                            .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetLastFiveOrdersWithUser()
        {
            //return await _context.Orders.OrderByDescending(o => o.OrderDate).Include(o => o.User).Take(5).AsNoTracking().ToListAsync();
            return await _context.Orders.Where(o => o.IsPaid == true).OrderByDescending(o => o.OrderDate).Include(o => o.User).Take(5).AsNoTracking().ToListAsync();
        }
    }
}