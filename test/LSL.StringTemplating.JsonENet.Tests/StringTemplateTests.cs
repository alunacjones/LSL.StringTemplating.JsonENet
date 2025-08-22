using System;
using System.Text.Json.Nodes;
using FluentAssertions;

namespace LSL.StringTemplating.JsonENet.Tests;

public class StringTemplateTests
{
    [Test]
    public void Ctor_GivenANullJsonNode_ItShouldThrowAnException()
    {
        new Action(() => _ = new StringTemplate(null))
            .Should()
            .ThrowExactly<ArgumentNullException>();
    }

    [Test]
    public void Ctor_GivenAnInvalidNodeItShouldThrowAnException()
    {
        new Action(() => _ = new StringTemplate(JsonNode.Parse("[]")))
            .Should()
            .ThrowExactly<ArgumentException>();
    }

    [Test]
    public void ToStringWithData_ShouldProduceTheExpectedResult()
    {
        var data = JsonNode.Parse(
            """
            {
                "id": 12345
            }
            """
        );

        var sut = new StringTemplate("\"v1/test/${id}\"");

        sut.ToString(data).Should().Be("v1/test/12345");
    }

    [Test]
    public void ToString_WithJsonETemplate_ShouldProduceTheExpectedResult()
    {
        var data = JsonNode.Parse(
            """
            {
                "id": 12345
            }
            """
        );

        var sut = new StringTemplate(JsonNode.Parse(
            """
            {
                "$let": { "double": { "$eval": "id * 2" } },
                "in": "v1/test/${double}"
            }
            """
        ));

        sut.ToString(data).Should().Be("v1/test/24690");
    }

    [Test]
    public void ToString_WithJsonETemplateThatDoesNotResultInAString_ShouldThrowAnException()
    {
        var data = JsonNode.Parse(
            """
            {
                "id": 12345
            }
            """
        );

        var sut = new StringTemplate(JsonNode.Parse(
            """
            {
                "value": {
                    "$let": { "double": { "$eval": "id * 2" } },
                    "in": "v1/test/${double}"
                }
            }
            """
        ));

        new Action(() => sut.ToString(data).Should().Be("v1/test/24690"))
            .Should()
            .ThrowExactly<FormatException>();
    }    

    [Test]
    public void ToString_WithNoParameters_ShouldProduceTheExpectedResult()
    {
        var sut = new StringTemplate("\"v1/test/${id}\"");

        sut.ToString().Should().Be("\"v1/test/${id}\"");
    }
}