using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entity
{
    public class Blog : BaseEntity
    {
        public string? ImageUrl { get; set; }
        public string? Title { get; set; }

        public string? CardDescription { get; set; }
        public string? PageDescription { get; set; }
        public bool Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? BlogCategoryId { get; set; }
        public BlogCategory BlogCategory { get; set; }

        //Meta Etiketleri
        public string? MetaDescription { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaKeywords { get; set; }
        public string? MetaCanonical { get; set; }
        public string? MetaOgTitle { get; set; }
        public string? MetaOgDescription { get; set; }
        public string? Url { get; set; }
    }
}