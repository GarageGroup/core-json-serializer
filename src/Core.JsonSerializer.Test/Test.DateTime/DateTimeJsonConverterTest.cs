using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;

namespace GarageGroup.Core.JsonSerializer.Test;

public static class DateTimeJsonConverterTest
{
    [Theory]
    [MemberData(nameof(ReadReturnsExpectedDateTimeTheoryData))]
    public static void Read_ReturnsExpectedDateTime(
        string? format, string json, DateTime expected)
    {
        var converter = new DateTimeJsonConverter(format);
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
        _ = reader.Read();

        var actual = converter.Read(
            reader: ref reader,
            typeToConvert: typeof(DateTime),
            options: new());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static void Read_ThrowsFormatException_WhenValueDoesNotMatchFormat()
    {
        var converter = new DateTimeJsonConverter("yyyy-MM-dd");

        var exception = Assert.Throws<FormatException>(ReadFromJson);

        Assert.NotNull(exception.Message);

        void ReadFromJson()
        {
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes("\"2024/04/15\""));
            _ = reader.Read();

            _ = converter.Read(
                reader: ref reader,
                typeToConvert: typeof(DateTime),
                options: new());
        }
    }

    [Theory]
    [InlineData(null, "\"04/15/2024 12:34:56\"")]
    [InlineData("", "\"04/15/2024 12:34:56\"")]
    [InlineData("yyyy-MM-dd", "\"2024-04-15\"")]
    [InlineData("yyyy-MM-dd HH:mm:ss", "\"2024-04-15 12:34:56\"")]
    public static void Write_WritesExpectedJson(
        string? format, string expectedJson)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        var converter = new DateTimeJsonConverter(format);

        converter.Write(
            writer: writer,
            date: new(2024, 4, 15, 12, 34, 56),
            options: new());

        writer.Flush();
        var actualJson = Encoding.UTF8.GetString(stream.ToArray());

        Assert.Equal(expectedJson, actualJson);
    }

    public static TheoryData<string?, string, DateTime> ReadReturnsExpectedDateTimeTheoryData
        =>
        new()
        {
            { null, "\"2024-04-15T12:34:56\"", new(2024, 4, 15, 12, 34, 56, DateTimeKind.Unspecified) },
            { string.Empty, "\"2024-04-15T12:34:56\"", new(2024, 4, 15, 12, 34, 56, DateTimeKind.Unspecified) },
            { "yyyy-MM-dd", "\"2024-04-15\"", new(2024, 4, 15, 0, 0, 0, DateTimeKind.Unspecified) },
            { "yyyy-MM-dd HH:mm:ss", "\"2024-04-15 12:34:56\"", new(2024, 4, 15, 12, 34, 56, DateTimeKind.Unspecified) }
        };
}
