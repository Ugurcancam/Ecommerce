using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entity
{
    public class Favorite : BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}