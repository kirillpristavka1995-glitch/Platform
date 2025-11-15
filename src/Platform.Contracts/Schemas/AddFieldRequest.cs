using Platform.Core.Schemas;

namespace Platform.Contracts.Schemas;

public record AddFieldRequest(
    string FieldName,
    JsonSchemaType? Type,
    bool Required = false,
    string? Ref = null
);