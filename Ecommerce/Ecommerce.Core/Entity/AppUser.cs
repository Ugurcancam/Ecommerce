using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Core.Entity
{
    public class AppUser : IdentityUser<string>
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        // İlişkisel
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Basket> Baskets { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}