using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetProductsWithCategory();
        Task<List<Product>> GetActiveProductsWithCategory();
        Task<List<Product>> GetSimilarProducts(int categoryId);
    }
}