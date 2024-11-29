namespace dotnet_mvc.DTOs
{
    public class CategoryReadDto
    {
        public Guid CategoryId {get; set; }
        public string Name {get; set; } = string.Empty;
        public string? Description {get; set; }
        // public DateTime CreatedAt {get; set;}
    }
}