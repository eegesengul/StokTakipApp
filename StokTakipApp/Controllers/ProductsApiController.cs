using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using stoktakip.DatabaseSpecific;
using stoktakip.EntityClasses;
using stoktakip.HelperClasses;
using StokTakipApp.Models;

[ApiController]
[Route("api/[controller]")]
public class ProductsApiController : ControllerBase
{
    private readonly IConfiguration _config;
    public ProductsApiController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        using var adapter = new DataAccessAdapter(connStr);
        var products = new EntityCollection<ProductEntity>();
        adapter.FetchEntityCollection(products, null);

        var result = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            CategoryId = p.CategoryId,
            StockAmount = p.StockAmount,
            PurchasePrice = p.PurchasePrice,
            SalePrice = p.SalePrice
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        using var adapter = new DataAccessAdapter(connStr);
        var product = new ProductEntity(id);
        if (adapter.FetchEntity(product))
        {
            var dto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                StockAmount = product.StockAmount,
                PurchasePrice = product.PurchasePrice,
                SalePrice = product.SalePrice
            };
            return Ok(dto);
        }
        return NotFound();
    }

    [HttpPost]
    public IActionResult Create([FromBody] ProductDto dto)
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        using var adapter = new DataAccessAdapter(connStr);
        var product = new ProductEntity
        {
            Name = dto.Name,
            CategoryId = dto.CategoryId,
            StockAmount = dto.StockAmount,
            PurchasePrice = dto.PurchasePrice,
            SalePrice = dto.SalePrice
        };
        adapter.SaveEntity(product, true);

        dto.Id = product.Id;
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, dto);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ProductDto dto)
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        using var adapter = new DataAccessAdapter(connStr);
        var product = new ProductEntity(id);
        if (!adapter.FetchEntity(product))
            return NotFound();

        product.Name = dto.Name;
        product.CategoryId = dto.CategoryId;
        product.StockAmount = dto.StockAmount;
        product.PurchasePrice = dto.PurchasePrice;
        product.SalePrice = dto.SalePrice;

        adapter.SaveEntity(product, false);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        using var adapter = new DataAccessAdapter(connStr);
        var product = new ProductEntity(id);
        if (!adapter.FetchEntity(product))
            return NotFound();
        adapter.DeleteEntity(product);
        return NoContent();
    }
}