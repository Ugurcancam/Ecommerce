// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace Ecommerce.Core.Entity
// {
//     public class Order : BaseEntity
//     {
//         public string OrderNumber { get; set; }
//         public DateTime OrderDate { get; set; }
//         public string Address { get; set; }
//         public string City { get; set; }
//         public string? District { get; set; }
//         public string? Phone { get; set; }

//         //İlişkisel
//         public string UserId { get; set; }
//         public AppUser User { get; set; }
//         public ICollection<OrderItem> OrderItems { get; set; }
//     }
// }