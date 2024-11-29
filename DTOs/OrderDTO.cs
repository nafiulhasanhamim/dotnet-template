namespace dotnet_mvc.DTOs
{
    public class OrderCreateDto
    {
        // public string UserId { get; set; }
        public List<OrderedProductCreateDto> OrderedProducts { get; set; } = new List<OrderedProductCreateDto>();
    }
    public class OrderReadDto
    {
        public Guid OrderId { get; set; } // Order ID
        public DateTime OrderDate { get; set; } // Date of the order
        public UserDto User { get; set; } = null!; // Information about the user who placed the order
        public List<OrderedProductReadDto> OrderedProducts { get; set; } = new List<OrderedProductReadDto>();
    }
    public class OrderUpdateDto
    {
        public List<OrderedProductUpdateDto> OrderedProducts { get; set; } = new List<OrderedProductUpdateDto>();
    }
    public class UserDto
    {
        public string UserId { get; set; } = null!; // User ID
        public string Email { get; set; } = null!; // User email
    }


}