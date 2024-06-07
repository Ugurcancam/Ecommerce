using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;
using Ecommerce.Core.Repositories;
using Ecommerce.Core.Services;
using Ecommerce.Core.UnitOfWorks;

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
            // Kullanıcının sepetini alır. Eğer kullanıcıya ait bir sepet yoksa 'null' döner.
            var basket = await _basketRepository.GetBasketByUserIdAsync(userId);
            // Eğer kullanıcının bir sepeti yoksa, yeni bir sepet oluşturur ve veritabanına ekler.
            if (basket == null)
            {
                basket = new Basket { UserId = userId };
                await _basketRepository.AddAsync(basket);
                await _unitOfWork.CommitAsync();
            }
            // Sepete eklenmek istenilen ürünün (productId) olup olmadığını kontrol eder.
            var basketItem = basket.BasketItems.FirstOrDefault(ci => ci.ProductId == productId);
            // Eğer ürün sepette varsa, ürünün miktarını arttırır.
            if (basketItem != null)
            {
                basketItem.Quantity += quantity;
                _basketItemRepository.Update(basketItem);
                await _unitOfWork.CommitAsync();
            }
            // Eğer ürün sepette yoksa, yeni bir ürün olarak ekler.
            else
            {
                basketItem = new BasketItem
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _basketItemRepository.AddAsync(basketItem);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task RemoveFromBasketAsync(string userId, int productId)
        {
            // Kullanıcının sepetini alır. Eğer kullanıcıya ait bir sepet yoksa 'null' döner.
            var basket = await _basketRepository.GetBasketByUserIdAsync(userId);
            if (basket != null)
            {
                // Sepetten çıkartılmak istenilen ürünün (productId) olup olmadığını kontrol eder.
                var basketItem = basket.BasketItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (basketItem != null)
                {
                    // Eğer ürün sepette varsa, ürünü sepetten çıkartır.
                    _basketItemRepository.Remove(basketItem);
                    await _unitOfWork.CommitAsync();
                }
            }
        }

        public async Task ReduceQuantityAsync(string userId, int productId)
        {
            // Kullanıcının sepetini alır. Eğer kullanıcıya ait bir sepet yoksa 'null' döner.
            var basket = await _basketRepository.GetBasketByUserIdAsync(userId);
            if (basket != null)
            {
                // Sepetten çıkartılmak istenilen ürünün (productId) olup olmadığını kontrol eder.
                var basketItem = basket.BasketItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (basketItem != null)
                {
                    // Eğer ürün sepette varsa, ürünün miktarını azaltır.
                    if (basketItem.Quantity > 1)
                    {
                        basketItem.Quantity -= 1;
                        _basketItemRepository.Update(basketItem);
                        await _unitOfWork.CommitAsync();
                    }
                    // Eğer ürün sepette 1 adet varsa, ürünü sepetten çıkartır.
                    else
                    {
                        _basketItemRepository.Remove(basketItem);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
        }
    }
}