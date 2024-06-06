using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Dtos;
using Ecommerce.Core.Dtos.Product;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Services
{
    public interface IProductService : IService<Product>
    {
        //For API
        Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategoryAPI();
        //For MVC
        Task<List<ProductWithCategoryDto>> GetProductsWithCategory();
        Task<List<Product>> GetActiveProductsWithCategory();
    }
}