using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Dtos
{
    public class BlogCategoryDto : BaseDto
    {
        public string Title { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}