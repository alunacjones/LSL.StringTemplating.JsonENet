using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using Json.JsonE;
using LSL.StringTemplating.JsonENet.Infrastructure;

namespace LSL.StringTemplating.JsonENet;

/// <summary>
/// An object that holds a string or a JSONE-based template
/// </summary>
public class StringTemplate
{
    /// <summary>
    /// The JSON node that the instance was initialised with
    /// </summary>
    public JsonNode JsonNode { get; private set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="jsonNode"></param>
    /// <exception cref="ArgumentException">This is thrown if the <see cref="System.Text.Json.Nodes.JsonNode"/> is not a string and not an object</exception>
    /// <exception cref="ArgumentNullException">This is thrown if the provided <paramref name="jsonNode"/> is <see langword="null"/></exception> 
    public StringTemplate(JsonNode jsonNode)
    {
        AssertIsAStringOrObject(jsonNode.AssertNotNull(nameof(jsonNode)));
        JsonNode = jsonNode;
    }

    /// <summary>
    /// Evaluates the string template to a string from the provided <see cref="System.Text.Json.Nodes.JsonNode"/>
    /// </summary>
    /// <param name="jsonEContext"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"/>
    public string ToString(JsonNode jsonEContext)
    {
        var result = JsonE.Evaluate(JsonNode, jsonEContext);

        if (result.GetValueKind() is not JsonValueKind.String)
        {
            throw new FormatException($"{JsonNode} did not evaluate to a string ({result})");
        }

        return result.ToString().Trim('\"');
    }

    /// <inheritdoc/>
    public override string ToString() => JsonNode.ToString();

    private void AssertIsAStringOrObject(JsonNode jsonNode)
    {
        var valueKind = jsonNode.AssertNotNull(nameof(jsonNode)).GetValueKind();
        if (valueKind != JsonValueKind.String && valueKind != JsonValueKind.Object)
        {
            throw new ArgumentException("Either a string or object JsonNode must be passed into the constructor", nameof(jsonNode));
        }
    }
}