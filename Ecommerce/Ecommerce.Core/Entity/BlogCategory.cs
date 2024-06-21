using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entity
{
    public class BlogCategory : BaseEntity
    {
        public string Title { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}