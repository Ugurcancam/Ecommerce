using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Dtos;
using Ecommerce.Core.Dtos.Category;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Services
{
    public interface ICategoryService : IService<Category>
    {
        Task<CustomResponseDto<CategoryWithProductsDto>> GetWithProductsByIdAsync(int categoryId);
    }
}