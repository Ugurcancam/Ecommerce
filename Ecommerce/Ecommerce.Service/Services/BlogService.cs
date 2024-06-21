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
    public class BlogService : Service<Blog>, IBlogService
    {
        private readonly IBlogRepository _blogrepository;
        public BlogService(IGenericRepository<Blog> genericRepository, IUnitOfWork unitOfWork, IBlogRepository blogRepository) : base(genericRepository, unitOfWork)
        {
            _blogrepository = blogRepository;
        }

        public async Task<IEnumerable<Blog>> GetAllWithCategoryAsync()
        {
           return await _blogrepository.GetAllWithCategoryAsync();
        }
    }
}