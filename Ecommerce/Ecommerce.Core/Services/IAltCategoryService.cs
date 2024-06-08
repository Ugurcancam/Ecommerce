using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Services
{
    public interface IAltCategoryService : IService<AltCategory>
    {
        Task<IEnumerable<AltCategory>> GetAllWithCategoryAsync();
    }
}