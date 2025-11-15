using Platform.Contracts.Schemas;
using Platform.Core.Schemas;

namespace Platform.Application.Schemas;

public interface ISchemaService
{
    Task<JsonSchema> CreateSchemaAsync(CreateSchemaRequest request);
    Task<JsonSchema> AddFieldAsync(string schemaName, AddFieldRequest request);
    Task<IEnumerable<string>> GetSchemasAsync();
    Task<Dictionary<string, JsonSchemaProperty>> GetPropertiesAsync(string schemaName);
}