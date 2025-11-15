using System.Text.Json;
using System.Text.Json.Serialization;
using Platform.Core.Schemas;
using Platform.Application.Schemas;

namespace Platform.Infrastructure.Schemas;

public class FileJsonSchemaRepository : IJsonSchemaRepository
{
    private readonly string _folder = Path.Combine("Schemas");
    private readonly JsonSerializerOptions _options;

    public FileJsonSchemaRepository()
    {
        Directory.CreateDirectory(_folder);

        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    private string GetPath(string name) =>
        Path.Combine(_folder, $"{name}.json");

    public Task<bool> ExistsAsync(string name)
        => Task.FromResult(File.Exists(GetPath(name)));

    public async Task SaveAsync(JsonSchema schema)
    {
        var json = JsonSerializer.Serialize(schema, _options);
        await File.WriteAllTextAsync(GetPath(schema.Title!), json);
    }
    
    public async Task<JsonSchema?> LoadAsync(string name)
    {
        var path = GetPath(name);
        if (!File.Exists(path))
            return null;

        var json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<JsonSchema>(json, _options);
    }
    
    public Task<IEnumerable<string>> ListAsync()
    {
        var files = Directory
            .GetFiles(_folder, "*.json")
            .Select(Path.GetFileNameWithoutExtension);

        return Task.FromResult(files);
    }
}