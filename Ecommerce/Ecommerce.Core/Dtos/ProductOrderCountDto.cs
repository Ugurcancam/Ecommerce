using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Core.Dtos
{
    public class ProductOrderCountDto
    {
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int OrderCount { get; set; }
        public double Price { get; set; }
        public double Revenue { get; set; }
        public bool Status { get; set; }
        public int Stock { get; set; }
        public bool InStock { get; set; }
    }
}