using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_mvc.Models
{
    public class Order
    {
        public Guid OrderId { get; set; } // Primary Key
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; } = null!; // Foreign Key
        public ApplicationUser User { get; set; } = null!;
        public ICollection<OrderedProduct> OrderedProducts { get; set; } = new List<OrderedProduct>();
}

}