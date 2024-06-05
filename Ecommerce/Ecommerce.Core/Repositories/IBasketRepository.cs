using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Repositories
{
    public interface IBasketRepository : IGenericRepository<Basket>
    {
        Task<Basket> GetBasketByUserIdAsync(string userId);
    }
}