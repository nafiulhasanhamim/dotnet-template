// Controllers/ProductsController.cs
using dotnet_mvc.DTOs;
using dotnet_mvc.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
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
        return NoContent();
    }
}
