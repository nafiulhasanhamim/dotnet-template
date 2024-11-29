using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_mvc.DTOs
{
    public class OrderedProductCreateDto
    {
        public Guid ProductId { get; set; } // ID of the product
        public int Quantity { get; set; } // Quantity of the product
    }
    public class OrderedProductReadDto
    {
        public Guid OrderedProductId { get; set; } // ID of the ordered product
        public int Quantity { get; set; } // Quantity of the product
        public ProductReadDto Product { get; set; } = null!; // Details of the product
    }
    public class OrderedProductUpdateDto
    {
        public Guid OrderedProductId { get; set; } // ID of the ordered product
        public int Quantity { get; set; } // Updated quantity of the product
    }

}