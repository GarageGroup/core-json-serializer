using Xunit;

namespace GarageGroup.Core.JsonSerializer.Test;

public sealed class JsonStringMaxLengthAttributeTest
{
    [Fact]
    public static void Serialize_TruncatesStringsByAttributeMaxLength()
    {
        var model = new TestModel
        {
            ShortText = "abcdef",
            LongText = "abcdefghij",
            NullableText = null
        };

        var actualJson = System.Text.Json.JsonSerializer.Serialize(model);

        Assert.Equal(
            "{\"ShortText\":\"abc\",\"LongText\":\"abcde\",\"NullableText\":null}",
            actualJson);
    }

    [Fact]
    public static void Deserialize_TruncatesStringsByAttributeMaxLength()
    {
        const string json = "{\"ShortText\":\"abcdef\",\"LongText\":\"abcdefghij\",\"NullableText\":\"qrstuv\"}";

        var actualModel = System.Text.Json.JsonSerializer.Deserialize<TestModel>(json);

        Assert.NotNull(actualModel);
        Assert.Equal("abc", actualModel.ShortText);
        Assert.Equal("abcde", actualModel.LongText);
        Assert.Equal("qrs", actualModel.NullableText);
    }

    private sealed record class TestModel
    {
        [JsonStringMaxLength(3)]
        public string? ShortText { get; init; }

        [JsonStringMaxLength(5)]
        public string? LongText { get; init; }

        [JsonStringMaxLength(3)]
        public string? NullableText { get; init; }
    }
}