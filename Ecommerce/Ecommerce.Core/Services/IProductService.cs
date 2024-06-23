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
        //For MVC
        // Pagination support for MVC
        Task<(List<ProductWithCategoryDto> Products, int TotalCount)> GetProductsWithCategory(int pageNumber, int pageSize);
        Task<List<Product>> GetActiveProductsWithCategory();
        Task<List<Product>> GetSimilarProducts(int categoryId);
        Task<int> GetTotalProductsCount();
    }
}