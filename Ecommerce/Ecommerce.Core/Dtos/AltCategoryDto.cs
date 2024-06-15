using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Core.Dtos
{
    public class AltCategoryDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Entity.Category Category { get; set; }
        public IEnumerable<Entity.Category> Categories { get; set; }
        public string Name { get; set; }
    }
}