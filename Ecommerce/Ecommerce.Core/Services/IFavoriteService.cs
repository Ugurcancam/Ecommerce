using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Services
{
    public interface IFavoriteService
    {
        Task AddFavorite(string userId, int productId);
        Task RemoveFavorite(string userId, int productId);
        Task<IEnumerable<Favorite>> GetFavorites(string userId);
    }
}