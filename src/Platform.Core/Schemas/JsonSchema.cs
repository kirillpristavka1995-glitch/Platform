using System.Text.Json.Serialization;

namespace Platform.Core.Schemas;

public class JsonSchema
{
    [JsonPropertyName("$schema")]
    public string? Schema { get; set; }

    [JsonPropertyName("$id")]
    public string? Id { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonSchemaTypeConverter))]
    public JsonSchemaType[] Types { get; set; } = { JsonSchemaType.Object };

    [JsonInclude]
    [JsonPropertyName("properties")]
    public Dictionary<string, JsonSchemaProperty> Properties { get; private set; }
        = new();

    [JsonInclude]
    [JsonPropertyName("required")]
    public List<string> Required { get; private set; }
        = new();

    [JsonInclude]
    [JsonPropertyName("$defs")]
    public Dictionary<string, JsonSchema> Defs { get; private set; }
        = new();

    public JsonSchema(string name)
    {
        Schema = "https://json-schema.org/draft/2020-12/schema";
        Id     = $"schemas/{name}.json";
        Title  = name;
    }

    public JsonSchema() { }
}