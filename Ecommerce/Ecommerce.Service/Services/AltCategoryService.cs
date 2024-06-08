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
    public class AltCategoryService : Service<AltCategory>, IAltCategoryService
    {
        private readonly IAltCategoryRepository _altCategoryRepository;
        public AltCategoryService(IGenericRepository<AltCategory> genericRepository, IUnitOfWork unitOfWork, IAltCategoryRepository altCategoryRepository) : base(genericRepository, unitOfWork)
        {
            _altCategoryRepository = altCategoryRepository;
        }

        public Task<IEnumerable<AltCategory>> GetAllWithCategoryAsync()
        {
            return _altCategoryRepository.GetAllWithCategoryAsync();
        }

    }
}