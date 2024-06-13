using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;
using Ecommerce.Core.Repositories;
using Ecommerce.Core.Services;
using Ecommerce.Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore.Storage;

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
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var usersFavorites = await _favoriteRepository.GetFavoritesByUserIdAsync(userId);
                var favorite = new Favorite
                {
                    UserId = userId,
                    ProductId = productId
                };

                if (usersFavorites != null)
                {
                    // Check if the product is already in the user's favorites.
                    var isExist = usersFavorites.FirstOrDefault(f => f.ProductId == productId);
                    if (isExist != null)
                    {
                        return;
                    }
                }

                await _favoriteRepository.AddAsync(favorite);
                await _unitOfWork.CommitAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task RemoveFavorite(string userId, int productId)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var usersFavorites = await _favoriteRepository.GetFavoritesByUserIdAsync(userId);
                if (usersFavorites != null)
                {
                    var favorite = usersFavorites.FirstOrDefault(f => f.ProductId == productId);
                    if (favorite != null)
                    {
                        _favoriteRepository.Remove(favorite);
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
