using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using stoktakip.DatabaseSpecific;
using stoktakip.EntityClasses;
using stoktakip.HelperClasses;

[ApiController]
[Route("api/[controller]")]
public class CategoryApiController : ControllerBase
{
    private readonly IConfiguration _config;

    public CategoryApiController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        using var adapter = new DataAccessAdapter(connStr);
        var categories = new EntityCollection<CategoryEntity>();
        adapter.FetchEntityCollection(categories, null);

        var result = categories.Select(c => new
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        using var adapter = new DataAccessAdapter(connStr);
        var category = new CategoryEntity(id);
        if (adapter.FetchEntity(category))
        {
            return Ok(new
            {
                Id = category.Id,
                Name = category.Name
            });
        }
        return NotFound();
    }

    [HttpPost]
    public IActionResult Create([FromBody] CategoryEntity entity)
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        using var adapter = new DataAccessAdapter(connStr);
        adapter.SaveEntity(entity, true);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] CategoryEntity entity)
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        using var adapter = new DataAccessAdapter(connStr);
        var category = new CategoryEntity(id);
        if (!adapter.FetchEntity(category))
            return NotFound();

        category.Name = entity.Name;
        adapter.SaveEntity(category, false);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        using var adapter = new DataAccessAdapter(connStr);

        var category = new CategoryEntity(id);
        if (!adapter.FetchEntity(category))
            return NotFound();

        adapter.DeleteEntity(category);
        return NoContent();
    }
}