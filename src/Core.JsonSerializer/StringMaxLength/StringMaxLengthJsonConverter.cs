using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GGroupp;

public sealed class StringMaxLengthJsonConverter : JsonConverter<string?>
{
    private readonly int maxLength;

    public StringMaxLengthJsonConverter(int maxLength)
        =>
        this.maxLength = maxLength;

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        var actualValue = value.CutOff(maxLength);
        writer.WriteStringValue(actualValue);
    }

    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        =>
        reader.GetString().CutOff(maxLength);
}