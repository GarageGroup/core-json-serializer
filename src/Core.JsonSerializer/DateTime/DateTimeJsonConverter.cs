using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GGroupp;

public sealed class DateTimeJsonConverter : JsonConverter<DateTime>
{
    private readonly string? format;

    public DateTimeJsonConverter(string? format)
        =>
        this.format = format;

    public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
    {
        var stringValue = string.IsNullOrEmpty(format) switch
        {
            true => date.ToString(CultureInfo.InvariantCulture),
            _ => date.ToString(format, CultureInfo.InvariantCulture)
        };

        writer.WriteStringValue(stringValue);
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var text = reader.GetString() ?? string.Empty;

        if (string.IsNullOrEmpty(format))
        {
            return DateTime.Parse(text, CultureInfo.InvariantCulture);
        }

        return DateTime.ParseExact(text, format, CultureInfo.InvariantCulture);
    }
}