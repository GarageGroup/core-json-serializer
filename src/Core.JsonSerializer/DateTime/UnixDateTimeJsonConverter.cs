using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup;

public sealed class UnixDateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.Number)
        {
            throw new JsonException("Token type must be Number");
        }

        var unixTimeSeconds = reader.GetInt64();
        return DateTime.UnixEpoch.AddSeconds(unixTimeSeconds);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var unixTimeSeconds = new DateTimeOffset(value).ToUnixTimeSeconds();
        writer.WriteNumberValue(unixTimeSeconds);
    }
}