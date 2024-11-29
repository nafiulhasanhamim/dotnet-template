using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_mvc.DTOs
{
public class ProductCreateDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; } // Foreign Key
    // public int Id { get; set; } // Foreign Key
}

public class ProductReadDto
{
    public Guid ProductId {get; set; }
    // public Guid Id {get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public CategoryReadDto Category { get; set; } = null!;
}

public class ProductUpdateDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    // public int CategoryId { get; set; }
}

}