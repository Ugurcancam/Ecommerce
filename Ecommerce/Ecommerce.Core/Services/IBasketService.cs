using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Services
{
    public interface IBasketService
    {
        Task<Basket> GetBasketByUserIdAsync(string userId);
        Task AddToBasketAsync(string userId, int productId, int quantity);
        Task RemoveFromBasketAsync(string userId, int productId);
        Task ReduceQuantityAsync(string userId, int productId);
    }
}