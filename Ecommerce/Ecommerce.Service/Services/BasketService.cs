using System;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;
using Ecommerce.Core.Repositories;
using Ecommerce.Core.Services;
using Ecommerce.Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ecommerce.Service.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<BasketItem> _basketItemRepository;

        public BasketService(IUnitOfWork unitOfWork, IBasketRepository basketRepository, IGenericRepository<BasketItem> basketItemRepository)
        {
            _basketRepository = basketRepository;
            _basketItemRepository = basketItemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Basket> GetBasketByUserIdAsync(string userId)
        {
            return await _basketRepository.GetBasketByUserIdAsync(userId);
        }

        public async Task AddToBasketAsync(string userId, int productId, int quantity)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var basket = await _basketRepository.GetBasketByUserIdAsync(userId);
                if (basket == null)
                {
                    basket = new Basket { UserId = userId };
                    await _basketRepository.AddAsync(basket);
                    await _unitOfWork.CommitAsync();
                }
                if (basket.BasketItems == null)
                {
                    basket.BasketItems = new List<BasketItem>();
                }

                var basketItem = basket.BasketItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (basketItem != null)
                {
                    basketItem.Quantity += quantity;
                    _basketItemRepository.Update(basketItem);
                }
                else
                {
                    basketItem = new BasketItem
                    {
                        BasketId = basket.Id,
                        ProductId = productId,
                        Quantity = quantity
                    };
                    await _basketItemRepository.AddAsync(basketItem);
                }
                await _unitOfWork.CommitAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task RemoveFromBasketAsync(string userId, int productId)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var basket = await _basketRepository.GetBasketByUserIdAsync(userId);
                if (basket != null)
                {
                    var basketItem = basket.BasketItems.FirstOrDefault(ci => ci.ProductId == productId);
                    if (basketItem != null)
                    {
                        _basketItemRepository.Remove(basketItem);
                        await _unitOfWork.CommitAsync();
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ReduceQuantityAsync(string userId, int productId)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var basket = await _basketRepository.GetBasketByUserIdAsync(userId);
                if (basket != null)
                {
                    var basketItem = basket.BasketItems.FirstOrDefault(ci => ci.ProductId == productId);

                    if (basketItem != null)
                    {
                        if (basketItem.Quantity > 1)
                        {
                            basketItem.Quantity -= 1;
                            _basketItemRepository.Update(basketItem);
                        }
                        else
                        {
                            _basketItemRepository.Remove(basketItem);
                        }
                        await _unitOfWork.CommitAsync();
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
