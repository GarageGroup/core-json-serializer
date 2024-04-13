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
public sealed class JsonStringMaxLengthAttribute(int maxLength) : JsonConverterAttribute
{
    public override JsonConverter? CreateConverter(Type typeToConvert)
        =>
        new StringMaxLengthJsonConverter(maxLength);
}