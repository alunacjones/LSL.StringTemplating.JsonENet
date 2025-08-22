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
    private readonly JsonNode _node;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="jsonNode"></param>
    /// <exception cref="ArgumentException">This is thrown if the <see cref="JsonNode"/> is not a string and not an object</exception>
    /// <exception cref="ArgumentNullException">This is thrown if the provided <paramref name="jsonNode"/> is <see langword="null"/></exception> 
    public StringTemplate(JsonNode jsonNode)
    {
        AssertIsAStringOrObject(jsonNode.AssertNotNull(nameof(jsonNode)));
        _node = jsonNode;
    }

    /// <summary>
    /// Evaluates the string template to a string from the provided <see cref="JsonNode"/>
    /// </summary>
    /// <param name="jsonEContext"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"/>
    public string ToString(JsonNode jsonEContext)
    {
        var result = JsonE.Evaluate(_node, jsonEContext);

        if (result.GetValueKind() is not JsonValueKind.String)
        {
            throw new FormatException($"{_node} did not evaluate to a string ({result})");
        }

        return result.ToString().Trim('\"');
    }

    /// <inheritdoc/>
    public override string ToString() => _node.ToString();

    private void AssertIsAStringOrObject(JsonNode jsonNode)
    {
        var valueKind = jsonNode.AssertNotNull(nameof(jsonNode)).GetValueKind();
        if (valueKind != JsonValueKind.String && valueKind != JsonValueKind.Object)
        {
            throw new ArgumentException("Either a string or object JsonNode must be passed into the constructor", nameof(jsonNode));
        }
    }
}