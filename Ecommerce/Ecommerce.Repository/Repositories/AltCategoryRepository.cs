using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;
using Ecommerce.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repository.Repositories
{
    public class AltCategoryRepository : GenericRepository<AltCategory>, IAltCategoryRepository
    {
        private readonly DbSet<AltCategory> _dbSet;
        public AltCategoryRepository(AppDbContext context) : base(context)
        {
            _dbSet = context.Set<AltCategory>();
        }

        public async Task<IEnumerable<AltCategory>> GetAllWithCategoryAsync()
        {
            return await _context.AltCategories.Include(x => x.Category).ToListAsync();
        }

    }
}