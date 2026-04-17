using System;
using Xunit;

namespace GarageGroup.Core.JsonSerializer.Test;

public static class JsonDateTimeAttributeTest
{
    [Fact]
    public static void Serialize_FormatsDateTimesByAttributeFormat()
    {
        var model = new TestModel
        {
            DateOnly = new(2024, 4, 15, 12, 34, 56),
            DateTime = new(2024, 4, 15, 12, 34, 56),
            Default = new(2024, 4, 15, 12, 34, 56)
        };

        var actualJson = System.Text.Json.JsonSerializer.Serialize(model);

        Assert.Equal(
            "{\"DateOnly\":\"2024-04-15\",\"DateTime\":\"2024-04-15 12:34:56\",\"Default\":\"04/15/2024 12:34:56\"}",
            actualJson);
    }

    [Fact]
    public static void Deserialize_ParsesDateTimesByAttributeFormat()
    {
        const string json
            =
            "{\"DateOnly\":\"2024-04-15\",\"DateTime\":\"2024-04-15 12:34:56\",\"Default\":\"04/15/2024 12:34:56\"}";

        var actualModel = System.Text.Json.JsonSerializer.Deserialize<TestModel>(json);

        Assert.NotNull(actualModel);

        Assert.Equal(new(2024, 4, 15, 0, 0, 0), actualModel.DateOnly);
        Assert.Equal(new(2024, 4, 15, 12, 34, 56), actualModel.DateTime);
        Assert.Equal(new(2024, 4, 15, 12, 34, 56), actualModel.Default);
    }

    private sealed record class TestModel
    {
        [JsonDateTime("yyyy-MM-dd")]
        public DateTime DateOnly { get; init; }

        [JsonDateTime("yyyy-MM-dd HH:mm:ss")]
        public DateTime DateTime { get; init; }

        [JsonDateTime(null)]
        public DateTime Default { get; init; }
    }
}
