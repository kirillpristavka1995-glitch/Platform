using Microsoft.AspNetCore.Mvc;
using Platform.Application.Schemas;
using Platform.Contracts.Schemas;

namespace Platform.Web.Controllers;

[ApiController]
[Route("api/schemas")]
public class SchemasController : ControllerBase
{
    private readonly ISchemaService _service;

    public SchemasController(ISchemaService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSchemaRequest request)
    {
        var schema = await _service.CreateSchemaAsync(request);
        return Ok(schema); // ← отдаём доменную модель наружу
    }
    
    [HttpPost("{schemaName}/fields")]
    public async Task<IActionResult> AddField(
        string schemaName,
        [FromBody] AddFieldRequest request)
    {
        var updatedSchema = await _service.AddFieldAsync(schemaName, request);
        return Ok(updatedSchema);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetSchemas()
    {
        var schemas = await _service.GetSchemasAsync();
        return Ok(schemas);
    }
    
    [HttpGet("{schemaName}/properties")]
    public async Task<IActionResult> GetProperties(string schemaName)
    {
        var props = await _service.GetPropertiesAsync(schemaName);
        return Ok(props);
    }
    
    [HttpGet("{schemaName}")]
    public async Task<IActionResult> GetSchema(string schemaName)
    {
        var schema = await _service.GetSchemaAsync(schemaName);
        return Ok(schema); // сериализуется через JsonOptions
    }
}