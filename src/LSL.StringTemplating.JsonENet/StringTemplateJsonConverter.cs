using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Json.More;

namespace LSL.StringTemplating.JsonENet;

/// <summary>
/// A JSON converter for <see cref="StringTemplate"/> 
/// </summary>
public class StringTemplateJsonConverter : JsonConverter<StringTemplate>
{
    /// <inheritdoc/>
    public override StringTemplate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType switch
        {
            JsonTokenType.String => new StringTemplate(reader.GetString()),
            JsonTokenType.StartObject => new StringTemplate(JsonSerializer.Deserialize<JsonNode>(ref reader, options)),
            JsonTokenType other => throw new InvalidOperationException($"Found an invalid JsonTokenType of {other}. Expected one of Null, String or StartObject.")
        };

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, StringTemplate value, JsonSerializerOptions options) =>
        writer.WriteRawValue(value.JsonNode.AsJsonString());
}