using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Core.Entity;
using Ecommerce.Core.Repositories;
using Ecommerce.Core.Services;
using Ecommerce.Core.UnitOfWorks;

namespace Ecommerce.Service.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FavoriteService(IFavoriteRepository favoriteRepository, IUnitOfWork unitOfWork)
        {
            _favoriteRepository = favoriteRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<Favorite>> GetFavorites(string userId)
        {
            return _favoriteRepository.GetFavoritesByUserIdAsync(userId);
        }

        public async Task AddFavorite(string userId, int productId)
        {
            var usersFavorites = await _favoriteRepository.GetFavoritesByUserIdAsync(userId);
            var favorite = new Favorite
            {
                UserId = userId,
                ProductId = productId
            };
            if(usersFavorites != null)
            {
                // Kullanıcının favorileri arasında eklemek istenen ürün var mı diye kontrol ediyoruz.
                var isExist = usersFavorites.FirstOrDefault(f => f.ProductId == productId);
                // Eğer ürün favoriler arasında varsa, aksiyon almıyoruz.
                if(isExist != null)
                {
                    return;
                }
            }
            await _favoriteRepository.AddAsync(favorite);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveFavorite(string userId, int productId)
        {
            // Kullanıcının favorilerini alıyoruz.
            var usersFavorites = await _favoriteRepository.GetFavoritesByUserIdAsync(userId);
            if (usersFavorites != null)
            {
                // Kullanıcının favorileri arasında silinmek istenen ürün var mı diye kontrol ediyoruz.
                var favorite = usersFavorites.FirstOrDefault(f => f.ProductId == productId);
                if (favorite != null)
                {
                    // Eğer ürün favoriler arasında varsa, favorilerden kaldırıyoruz.
                    _favoriteRepository.Remove(favorite);
                    await _unitOfWork.CommitAsync();
                }
            }
        }
    }
}