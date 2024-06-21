using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Dtos;
using Ecommerce.Core.Entity;
using Ecommerce.Core.Repositories;
using Ecommerce.Core.Services;
using Ecommerce.Core.UnitOfWorks;
using Ecommerce.Repository;
using Microsoft.EntityFrameworkCore;
using static Ecommerce.Core.Entity.Order;

namespace Ecommerce.Service.Services
{
    public class OrderService : Service<Order>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBasketService _basketService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IGenericRepository<Order> genericRepository, IUnitOfWork unitOfWork, IOrderRepository orderRepository, IBasketService basketService, IUserService userService) : base(genericRepository, unitOfWork)
        {
            _orderRepository = orderRepository;
            _basketService = basketService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateOrderAsync(string userId, string paymentId, OrderDto orderDto)
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
                City = orderDto.City,
                District = orderDto.District,
                PostalCode = orderDto.PostalCode,
                OrderNote = orderDto.OrderNote,
                DeliveryAddress = orderDto.DeliveryAddress,
                BillingAddress = orderDto.BillingAddress,
                OrderState = OrderState.PaymentPending,
                PhoneNumber = orderDto.PhoneNumber,
                OrderNumber = "#" + new Random().Next(111111, 999999).ToString(),
                TotalAmount = basket.BasketItems.Sum(bi => bi.Quantity * bi.Product.Price),
                PaymentId = paymentId,
                OrderItems = basket.BasketItems.Select(bi => new OrderItem
                {
                    ProductId = bi.ProductId,
                    Product = bi.Product,
                    Quantity = bi.Quantity,
                    UnitPrice = bi.Product.Price
                }).ToList()
            };

            // Create a transaction for the order creation process
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {

                await _orderRepository.AddAsync(order);
                await _unitOfWork.CommitAsync();

                // Clear the basket after creating the order
                basket.BasketItems.Clear();
                await _unitOfWork.CommitAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return order;
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<Order> GetOrderDetailsByUserAsync(string userId, int orderId)
        {
            return await _orderRepository.GetOrderDetailsByUserAsync(userId, orderId);
        }
        public async Task<Order> GetOrderDetailsAsync(int orderId)
        {
            return await _orderRepository.GetOrderDetailsAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetAllWithUsers()
        {
            return await _orderRepository.GetAllWithUsers();
        }

        public async Task<Order> GetByPaymentIdAsync(string paymentId)
        {
            return await _orderRepository.GetByPaymentIdAsync(paymentId);
        }

        public async Task<IEnumerable<Order>> GetCargoPendingOrdersWithUser()
        {
            return await _orderRepository.GetCargoPendingOrdersWithUser();
        }

        public async Task<IEnumerable<Order>> GetShippedOrdersWithUser()
        {
            return await _orderRepository.GetShippedOrdersWithUser();
        }

        public async Task<IEnumerable<Order>> GetDeliveredOrdersWithUser()
        {
            return await _orderRepository.GetDeliveredOrdersWithUser();
        }

        public async Task<IEnumerable<CityOrderCountDto>> GetMostOrderedCities()
        {
            return await _orderRepository.GetMostOrderedCities();
        }

        public async Task<IEnumerable<ProductOrderCountDto>> GetMostOrderedProducts()
        {
            return await _orderRepository.GetMostOrderedProducts();
        }

        public async Task<IEnumerable<Order>> GetLastFiveOrdersWithUser()
        {
            return await _orderRepository.GetLastFiveOrdersWithUser();
        }
    }
}