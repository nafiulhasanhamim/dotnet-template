// Controllers/ProductsController.cs
using dotnet_mvc.DTOs;
using dotnet_mvc.Interfaces;
using dotnet_mvc.Services.Caching;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IRedisCacheService _cache;

    public ProductController(IProductService productService, IRedisCacheService cache)
    {
        _productService = productService;
        _cache = cache;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _cache.GetDataAsync<IEnumerable<ProductReadDto>>("products");
        if (products is not null)
        {
            return Ok(products);
        }
        products = await _productService.GetAllAsync();
        _cache.SetData("products", products);
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    // [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto productDto)
    {
        var product = await _productService.CreateAsync(productDto);
        _cache.RemoveData("products");
        return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
    }

    [HttpPut("{id:guid}")]
    // [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDto productDto)
    {
        var product = await _productService.UpdateAsync(id, productDto);
        return Ok(product);
    }

    [HttpDelete("{id:guid}")]
    // [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _productService.DeleteAsync(id);
        _cache.RemoveData("products");
        return NoContent();
    }
}
