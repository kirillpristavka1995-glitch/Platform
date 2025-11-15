using Platform.Core.Schemas;

namespace Platform.Application.Schemas;

public interface IJsonSchemaRepository
{
    Task<JsonSchema?> LoadAsync(string name);
    Task SaveAsync(JsonSchema schema);
    Task<bool> ExistsAsync(string name);
    Task<IEnumerable<string>> ListAsync();
}