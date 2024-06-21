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
    public class BlogCategoryService : Service<BlogCategory>, IBlogCategoryService
    {
        public BlogCategoryService(IGenericRepository<BlogCategory> genericRepository, IUnitOfWork unitOfWork) : base(genericRepository, unitOfWork)
        {
            
        }
    }
}