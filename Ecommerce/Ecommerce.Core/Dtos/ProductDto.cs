using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Dtos
{
    public class ProductDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public bool? InStock { get; set; }
        public bool? IsActive { get; set; }
        public string Color { get; set; } // ProductFeature property

        // public string? ImageUrl { get; set; }
        // public string? ImageUrl2 { get; set; }
        // public string? ImageUrl3 { get; set; }
        // public decimal? DiscountPrice { get; set; }
        // public bool? IsFavorite { get; set; }
        // public bool? IsBestSelling { get; set; }

        //İlişkisel
        public int CategoryId { get; set; }
        public Entity.Category category { get; set; }
        public IEnumerable<Entity.Category> Categories { get; set; }
        public int AltCategoryId { get; set; }
        public IEnumerable<AltCategory> AltCategories { get; set; }
        public Entity.Product Product { get; set; }
         public List<Entity.Product> SimilarProducts { get; set; }
    }
}