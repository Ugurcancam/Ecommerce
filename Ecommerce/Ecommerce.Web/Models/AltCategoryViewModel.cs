using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Web.Models
{
    public class AltCategoryViewModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public string Name { get; set; }
    }
}