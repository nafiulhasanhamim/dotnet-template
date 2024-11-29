namespace dotnet_mvc.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }
        // public int Id {get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; } // Foreign Key

        public Category Category { get; set; } = null!;
        public ICollection<OrderedProduct> OrderedProducts { get; set; } = new List<OrderedProduct>();

    }
}