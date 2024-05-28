using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entity
{
    public class Favorite
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Product Product { get; set; }
    }
}