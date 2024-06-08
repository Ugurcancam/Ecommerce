using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Repositories
{
    public interface IAltCategoryRepository : IGenericRepository<AltCategory>
    {
        Task<IEnumerable<AltCategory>> GetAllWithCategoryAsync();
    }
}