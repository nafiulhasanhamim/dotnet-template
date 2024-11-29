using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_mvc.Models
{
    public class OrderedProduct
    {
        public Guid OrderedProductId { get; set; } // Primary Key
        public Guid OrderId { get; set; } // Foreign Key
        public Guid ProductId { get; set; } // Foreign Key
        public int Quantity { get; set; }
        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}