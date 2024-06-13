using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;
using Ecommerce.Core.Repositories;
using Ecommerce.Core.Services;
using Ecommerce.Core.UnitOfWorks;
using Ecommerce.Repository;
using Microsoft.EntityFrameworkCore;
using static Ecommerce.Core.Entity.Order;

namespace Ecommerce.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IBasketService _basketService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(AppDbContext context, IUnitOfWork unitOfWork, IOrderRepository orderRepository, IBasketService basketService, IUserService userService)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _basketService = basketService;
            _userService = userService;
        }

        public async Task<Order> CreateOrderAsync(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            var basket = await _basketService.GetBasketByUserIdAsync(userId);

            if (basket == null || !basket.BasketItems.Any())
            {
                throw new InvalidOperationException("Basket is empty.");
            }

            var order = new Order
            {
                UserId = userId,
                User = user,
                OrderDate = DateTime.Now,
                TotalAmount = basket.BasketItems.Sum(bi => bi.Quantity * bi.Product.Price),
                OrderItems = basket.BasketItems.Select(bi => new OrderItem
                {
                    ProductId = bi.ProductId,
                    Product = bi.Product,
                    Quantity = bi.Quantity,
                    UnitPrice = bi.Product.Price
                }).ToList()
            };

            await _orderRepository.AddAsync(order);
            await _unitOfWork.CommitAsync();

            // Clear the basket after creating the order
            basket.BasketItems.Clear();
            await _unitOfWork.CommitAsync();

            return order;
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<Order> GetOrderDetailsAsync(string userId, int orderId)
        {
            return await _context.Orders
                                 .Where(order => order.UserId == userId && order.Id == orderId)
                                 .Include(order => order.OrderItems)
                                 .ThenInclude(item => item.Product)
                                 .FirstOrDefaultAsync();
        }

    }
}