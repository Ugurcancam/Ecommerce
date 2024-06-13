using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entity
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}