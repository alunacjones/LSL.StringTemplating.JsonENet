using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using FluentAssertions;

namespace LSL.StringTemplating.JsonENet.Tests;

public class StringTemplateJsonConverterTests
{
    [Test]
    public void Deserialise_GivenASimpleJsonString_ItShouldProduceTheExpectedResult()
    {
        var value = JsonSerializer.Deserialize<TestClass>(
            """
            {
                "Value": "test${id}"
            }
            """
        );

        value.Value.ToString().Should().Be("test${id}");
        value.Value.ToString(JsonNode.Parse(
            """
            {
                "id": 123
            }
            """))
            .Should()
            .Be("test123");
    }

    [Test]
    public void Deserialise_GivenANullJsonString_ItShouldProduceTheExpectedResult()
    {
        var value = JsonSerializer.Deserialize<TestClass>(
            """
            {
                "Value": null
            }
            """
        );

        value.Value.Should().BeNull();
    }    

    [Test]
    public void Deserialise_GivenAnUnsupportedToken_ItShouldProduceTheExpectedResult()
    {
        new Action(() => JsonSerializer.Deserialize<TestClass>(
            """
            {
                "Value": []
            }
            """
        ))
        .Should()
        .ThrowExactly<InvalidOperationException>();
    }        

    [Test]
    public void Deserialise_GivenAComplexValue_ItShouldProduceTheExpectedResult()
    {
        var value = JsonSerializer.Deserialize<TestClass>(
            """
            {
                "Value": { "$eval": "id * 2" }
            }
            """
        );

        value.Value.ToString().Should().Be(
            """
            {
              "$eval": "id * 2"
            }
            """.ReplaceLineEndings()
        );
    }

    [Test]
    public void Serialise_GivenAComplexValue_ItShouldProduceTheExpectedResult()
    {
        var value = new TestClass
        {
            Value = new StringTemplate(JsonNode.Parse(
                """
                {
                    "$eval": "stuff${id}"
                }
                """
            ))
        };

        JsonSerializer.Serialize(value).Should().Be("{\"Value\":{\"$eval\":\"stuff${id}\"}}");
    }

    [Test]
    public void Serialise_GivenAStringValue_ItShouldProduceTheExpectedResult()
    {
        var value = new TestClass
        {
            Value = new StringTemplate(JsonNode.Parse("\"test\""))
        };

        JsonSerializer.Serialize(value).Should().Be("{\"Value\":\"test\"}");
    }    

    internal class TestClass
    {
        [JsonConverter(typeof(StringTemplateJsonConverter))]
        public StringTemplate Value { get; set; }
    }
}