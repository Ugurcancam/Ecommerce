using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entity
{
    public class ProductFeature
    {
        public int Id { get; set; }
        public string Color { get; set; }

        //İlişkisel
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}