using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;

namespace GarageGroup.Core.JsonSerializer.Test;

public static class UnixDateTimeJsonConverterTest
{
    [Theory]
    [InlineData("0", 0L)]
    [InlineData("1", 1L)]
    [InlineData("-1", -1L)]
    [InlineData("1713139200", 1713139200L)]
    public static void Read_ReturnsExpectedDateTime(string json, long unixTimeSeconds)
    {
        var converter = new UnixDateTimeJsonConverter();
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
        _ = reader.Read();

        var actual = converter.Read(
            reader: ref reader,
            typeToConvert: typeof(DateTime),
            options: new());

        var expected = DateTime.UnixEpoch.AddSeconds(unixTimeSeconds);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("\"0\"")]
    [InlineData("null")]
    [InlineData("true")]
    public static void Read_ThrowsJsonException_WithExpectedMessage_WhenTokenTypeIsNotNumber(string json)
    {
        var converter = new UnixDateTimeJsonConverter();
        var exception = Assert.Throws<JsonException>(ReadFromJson);

        Assert.Equal("Token type must be Number", exception.Message);

        void ReadFromJson()
        {
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
            _ = reader.Read();

            _ = converter.Read(
                reader: ref reader,
                typeToConvert: typeof(DateTime),
                options: new());
        }
    }

    [Theory]
    [InlineData(0L, "0")]
    [InlineData(1L, "1")]
    [InlineData(-1L, "-1")]
    [InlineData(1713139200L, "1713139200")]
    public static void Write_WritesExpectedJson(long unixTimeSeconds, string expectedJson)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        var value = DateTime.UnixEpoch.AddSeconds(unixTimeSeconds);
        var converter = new UnixDateTimeJsonConverter();

        converter.Write(
            writer: writer,
            value: value,
            options: new());

        writer.Flush();
        var actualJson = Encoding.UTF8.GetString(stream.ToArray());

        Assert.Equal(expectedJson, actualJson);
    }
}