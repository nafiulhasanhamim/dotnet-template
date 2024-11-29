// Services/ProductService.cs
using AutoMapper;
using dotnet_mvc.data;
using dotnet_mvc.DTOs;
using dotnet_mvc.Interfaces;
using dotnet_mvc.Models;
using Microsoft.EntityFrameworkCore;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProductService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductReadDto>> GetAllAsync()
    {
        var products = await _context.Products.Include(p => p.Category).ToListAsync();
        return _mapper.Map<IEnumerable<ProductReadDto>>(products);
    }

    public async Task<ProductReadDto> GetByIdAsync(Guid id)
    {
        var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
        if (product == null) throw new KeyNotFoundException("Product not found");
        return _mapper.Map<ProductReadDto>(product);
    }

    public async Task<ProductReadDto> CreateAsync(ProductCreateDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return _mapper.Map<ProductReadDto>(product);
    }

    public async Task<ProductReadDto> UpdateAsync(Guid id, ProductUpdateDto productDto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) throw new KeyNotFoundException("Product not found");

        _mapper.Map(productDto, product);
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return _mapper.Map<ProductReadDto>(product);

    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) throw new KeyNotFoundException("Product not found");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}
