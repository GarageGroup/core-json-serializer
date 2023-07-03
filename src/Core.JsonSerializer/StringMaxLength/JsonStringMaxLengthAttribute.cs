using System;
using System.Text.Json.Serialization;

namespace GarageGroup;

[AttributeUsage(
    AttributeTargets.Class |
    AttributeTargets.Struct |
    AttributeTargets.Enum |
    AttributeTargets.Property |
    AttributeTargets.Field |
    AttributeTargets.Interface,
    AllowMultiple = false)]
public sealed class JsonStringMaxLengthAttribute : JsonConverterAttribute
{
    private readonly int maxLength;

    public JsonStringMaxLengthAttribute(int maxLength)
        =>
        this.maxLength = maxLength;

    public override JsonConverter? CreateConverter(Type typeToConvert)
        =>
        new StringMaxLengthJsonConverter(maxLength);
}