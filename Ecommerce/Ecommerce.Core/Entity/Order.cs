using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entity
{
    public class Order : BaseEntity
    {
        public string City { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string? OrderNote { get; set; }
        public string DeliveryAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        //İlişkisel
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}