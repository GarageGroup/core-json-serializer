using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup;

public sealed class StringMaxLengthJsonConverter(int maxLength) : JsonConverter<string?>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        =>
        reader.GetString().CutOff(maxLength);

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        var actualValue = value.CutOff(maxLength);
        writer.WriteStringValue(actualValue);
    }
}