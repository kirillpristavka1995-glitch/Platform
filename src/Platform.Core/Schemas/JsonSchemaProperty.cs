using System.Text.Json.Serialization;

namespace Platform.Core.Schemas;

public class JsonSchemaProperty
{
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonSchemaTypeConverter))]
    public JsonSchemaType[]? Types { get; set; }

    public string? Description { get; set; }

    [JsonPropertyName("$ref")]
    public string? Ref { get; set; }

    [JsonInclude]
    [JsonPropertyName("properties")]
    public Dictionary<string, JsonSchemaProperty>? Properties { get; private set; }

    [JsonInclude]
    [JsonPropertyName("required")]
    public List<string>? Required { get; private set; }

    public void SetType(JsonSchemaType type)
        => Types = new[] { type };

    public void AddProperty(string name, JsonSchemaProperty property)
    {
        Properties ??= new();
        Properties[name] = property;
    }

    public void AddRequired(string name)
    {
        Required ??= new();
        if (!Required.Contains(name))
            Required.Add(name);
    }
}