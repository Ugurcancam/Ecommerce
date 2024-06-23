using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Dtos.Product;

namespace Ecommerce.Web.Models
{
    public class ProductListViewModel
    {
        public List<ProductWithCategoryDto> Products { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }

}