using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Web.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        // public string? ImageUrl { get; set; }
        // public string? ImageUrl2 { get; set; }
        // public string? ImageUrl3 { get; set; }
        // public decimal? DiscountPrice { get; set; }
        // public bool? IsFavorite { get; set; }
        // public bool? IsBestSelling { get; set; }
        // public bool? InStock { get; set; }
        // public bool? IsActive { get; set; }

        //İlişkisel
        public int CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; }

    }
}