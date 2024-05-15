using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;
using Ecommerce.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetProductsWithCategory()
        {
            //Eagar Loading, datayı çekerken kategorisini de çeker.
            //Productları ilk çektiğimizde kategorileri de çekersek bu işlemi eager loading ile yapmış oluruz.
            return await _context.Products.Include(x => x.Category).ToListAsync();
        }
    }
}