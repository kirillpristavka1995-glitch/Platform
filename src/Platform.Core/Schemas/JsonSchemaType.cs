namespace Platform.Core.Schemas;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum JsonSchemaType
{
    Object,
    Array,
    String,
    Number,
    Integer,
    Boolean,
    Null
}