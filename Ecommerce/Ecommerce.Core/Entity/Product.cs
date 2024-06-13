using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entity
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? IsActive { get; set; }
        public bool? InStock { get; set; }

        //İlişkisel
        public int CategoryId { get; set; }
        public int? AltCategoryId { get; set; }
        public Category Category { get; set; }
        public AltCategory? AltCategory { get; set; }
        public ProductFeature ProductFeature { get; set; }
        public ICollection<Favorite> Favorites { get; set; } // Favori ile ilişki
        public ICollection<BasketItem> BasketItems { get; set; } // Sepetle ilişki
        public ICollection<OrderItem> OrderItems { get; set; } // Sipariş ile ilişki

        // İleride eklenecek özellikler
        // public string? ImageUrl { get; set; }
        // public string? ImageUrl2 { get; set; }
        // public string? ImageUrl3 { get; set; }
        // public decimal? DiscountPrice { get; set; }
        // public bool? IsFavorite { get; set; }
        // public bool? IsBestSelling { get; set; }


    }
}