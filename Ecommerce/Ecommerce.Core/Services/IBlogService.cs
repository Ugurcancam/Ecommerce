using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Services
{
    public interface IBlogService : IService<Blog>
    {
        Task<IEnumerable<Blog>> GetAllWithCategoryAsync();  
    }
}