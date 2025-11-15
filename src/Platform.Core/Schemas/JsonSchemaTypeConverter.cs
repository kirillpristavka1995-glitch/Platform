using System.Text.Json;
using System.Text.Json.Serialization;

namespace Platform.Core.Schemas;

public class JsonSchemaTypeConverter : JsonConverter<JsonSchemaType[]?>
{
    public override JsonSchemaType[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // "type": "string"
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString()!;
            return new[] { ParseType(value) };
        }

        // "type": ["string", "null"]
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            var list = new List<JsonSchemaType>();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.String)
                    list.Add(ParseType(reader.GetString()!));
            }
            return list.ToArray();
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, JsonSchemaType[]? value, JsonSerializerOptions options)
    {
        if (value == null || value.Length == 0)
        {
            writer.WriteNullValue();
            return;
        }

        if (value.Length == 1)
        {
            writer.WriteStringValue(TypeToString(value[0]));
            return;
        }

        writer.WriteStartArray();
        foreach (var t in value)
            writer.WriteStringValue(TypeToString(t));
        writer.WriteEndArray();
    }

    private static JsonSchemaType ParseType(string value) =>
        value.ToLower() switch
        {
            "object" => JsonSchemaType.Object,
            "array" => JsonSchemaType.Array,
            "string" => JsonSchemaType.String,
            "number" => JsonSchemaType.Number,
            "integer" => JsonSchemaType.Integer,
            "boolean" => JsonSchemaType.Boolean,
            "null" => JsonSchemaType.Null,
            _ => throw new JsonException($"Unknown JSON schema type: {value}")
        };

    private static string TypeToString(JsonSchemaType type) =>
        type.ToString().ToLower();
}