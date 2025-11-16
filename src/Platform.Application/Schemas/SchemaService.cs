using Platform.Contracts.Schemas;
using Platform.Core.Schemas;

namespace Platform.Application.Schemas;

public class SchemaService : ISchemaService
{
    private readonly IJsonSchemaRepository _repo;

    public SchemaService(IJsonSchemaRepository repo)
    {
        _repo = repo;
    }

    public async Task<JsonSchema> CreateSchemaAsync(CreateSchemaRequest request)
    {
        if (await _repo.ExistsAsync(request.Name))
            throw new InvalidOperationException($"Schema '{request.Name}' already exists.");

        var schema = new JsonSchema(request.Name);
        await _repo.SaveAsync(schema);
        return schema;
    }

    public async Task<JsonSchema> AddFieldAsync(string schemaName, AddFieldRequest request)
    {
        var schema = await _repo.LoadAsync(schemaName)
                     ?? throw new Exception("Schema not found.");

        var property = new JsonSchemaProperty();

        if (!string.IsNullOrWhiteSpace(request.Ref))
        {
            // === ЭТО РЕФЕРЕНС ===
            property.Ref = request.Ref;
        }
        else if (request.Type.HasValue)
        {
            // === ЭТО Обычный тип ===
            property.SetType(request.Type.Value);
        }
        else
        {
            throw new Exception("Field must have either 'type' or '$ref'.");
        }

        // Добавляем в properties
        schema.Properties[request.FieldName] = property;

        if (request.Required)
            schema.Required.Add(request.FieldName);

        await _repo.SaveAsync(schema);

        return schema;
    }
    
    public Task<IEnumerable<string>> GetSchemasAsync()
    {
        return _repo.ListAsync();
    }
    
    public async Task<Dictionary<string, JsonSchemaProperty>> GetPropertiesAsync(string schemaName)
    {
        var schema = await _repo.LoadAsync(schemaName)
                     ?? throw new Exception($"Schema '{schemaName}' not found.");

        return schema.Properties ?? new Dictionary<string, JsonSchemaProperty>();
    }
    
    public async Task<JsonSchema> GetSchemaAsync(string schemaName)
    {
        var schema = await _repo.LoadAsync(schemaName)
                     ?? throw new Exception($"Schema '{schemaName}' not found.");

        return schema;
    }
}