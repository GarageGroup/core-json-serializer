using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;

namespace GarageGroup.Core.JsonSerializer.Test;

public sealed class StringMaxLengthJsonConverterTest
{
    [Theory]
    [InlineData("\"abcdef\"", 3, "abc")]
    [InlineData("\"abcdef\"", 10, "abcdef")]
    [InlineData("\"abc\"", 3, "abc")]
    [InlineData("null", 5, null)]
    public static void Read_ReturnsExpectedValue(string json, int maxLength, string? expected)
    {
        var sut = new StringMaxLengthJsonConverter(maxLength);
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
        _ = reader.Read();

        var actual = sut.Read(
            reader: ref reader,
            typeToConvert: typeof(string),
            options: new());

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("abcdef", 3, "\"abc\"")]
    [InlineData("abcdef", 10, "\"abcdef\"")]
    [InlineData("abc", 3, "\"abc\"")]
    [InlineData(null, 5, "null")]
    public static void Write_WritesExpectedJson(string? value, int maxLength, string expectedJson)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        var sut = new StringMaxLengthJsonConverter(maxLength);
        sut.Write(
            writer: writer,
            value: value,
            options: new());

        writer.Flush();
        var actualJson = Encoding.UTF8.GetString(stream.ToArray());

        Assert.Equal(expectedJson, actualJson);
    }
}
