using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entity;

namespace Ecommerce.Core.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string? OrderNote { get; set; }
        public string DeliveryAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public double CargoPrice { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public string CVV { get; set; }
        public string OrderState { get; set; }
        //İlişkisel
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}