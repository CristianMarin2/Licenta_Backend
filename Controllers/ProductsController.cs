using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfCheckoutSystem.Data;
using SelfCheckoutSystem.Models;

namespace SelfCheckoutSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly SelfCheckoutDbContext _context;

    public ProductsController(SelfCheckoutDbContext context)
    {
        _context = context;
    }

    // POST: /api/products
    [HttpPost]
    public async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
    {
        var exists = await _context.Products.AnyAsync(p => p.Barcode == product.Barcode);
        if (exists)
            return Conflict("Produsul există deja.");

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { barcode = product.Barcode }, product);
    }

    // GET: /api/products?barcode=123
    [HttpGet]
    public async Task<ActionResult<Product>> GetProduct([FromQuery] string barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode))
            return BadRequest("Codul de bare este necesar.");

        var product = await _context.Products
            .Where(p => p.Barcode == barcode && p.IsActive)
            .FirstOrDefaultAsync();

        if (product == null)
            return NotFound("Produsul nu a fost găsit sau este inactiv.");

        return product;
    }

    // GET: /api/products/all
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        var products = await _context.Products
            .Where(p => p.IsActive)
            .ToListAsync();

        return Ok(products);
    }


    // PUT: /api/products/{barcode}
    [HttpPut("{barcode}")]
    public async Task<IActionResult> UpdateProduct(string barcode, [FromBody] Product updated)
    {
        if (barcode != updated.Barcode)
            return BadRequest("Codul de bare nu se potrivește.");

        var existing = await _context.Products.FindAsync(barcode);
        if (existing == null)
            return NotFound("Produsul nu a fost găsit.");

        existing.Name = updated.Name;
        existing.Price = updated.Price;
        existing.VatRate = updated.VatRate;
        existing.IsActive = updated.IsActive;

        await _context.SaveChangesAsync();
        return NoContent();
    }

// GET: /api/products/inactive
[HttpGet("inactive")]
public async Task<ActionResult<IEnumerable<Product>>> GetInactiveProducts()
{
    var products = await _context.Products
        .Where(p => !p.IsActive)
        .ToListAsync();

    return Ok(products);
}


}
