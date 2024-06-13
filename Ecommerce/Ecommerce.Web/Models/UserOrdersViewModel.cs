using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Web.Models
{
    public class UserOrdersViewModel
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public string ProductQuantity { get; set; }
        public string OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        //public string OrderStatus { get; set; }
    }
}