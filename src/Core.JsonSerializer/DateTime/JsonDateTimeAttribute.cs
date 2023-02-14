using System;
using System.Text.Json.Serialization;

namespace GGroupp;

[AttributeUsage(
    AttributeTargets.Class |
    AttributeTargets.Struct |
    AttributeTargets.Enum |
    AttributeTargets.Property |
    AttributeTargets.Field |
    AttributeTargets.Interface,
    AllowMultiple = false)]
public sealed class JsonDateTimeAttribute : JsonConverterAttribute
{
    private readonly string? format;

    public JsonDateTimeAttribute(string? format)
        =>
        this.format = format;

    public override JsonConverter? CreateConverter(Type typeToConvert)
        =>
        new DateTimeJsonConverter(format);
}