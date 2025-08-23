[![Build status](https://img.shields.io/appveyor/ci/alunacjones/lsl-stringtemplating-jsonenet.svg)](https://ci.appveyor.com/project/alunacjones/lsl-stringtemplating-jsonenet)
[![Coveralls branch](https://img.shields.io/coverallsCoverage/github/alunacjones/LSL.StringTemplating.JsonENet)](https://coveralls.io/github/alunacjones/LSL.StringTemplating.JsonENet)
[![NuGet](https://img.shields.io/nuget/v/LSL.StringTemplating.JsonENet.svg)](https://www.nuget.org/packages/LSL.StringTemplating.JsonENet/)

# LSL.StringTemplating.JsonENet

This library provides a `StringTemplate` class that encapsulates templating of a string using [JsonE.Net](https://www.nuget.org/packages/JsonE.Net)

A `System.Text.Json` converter is also provided to serialise and deserialise instances of `StringTemplate`. This is the `StringTemplateJsonConverter` class.

# StringTemplate

The following code shows a very basic usage of `StringTemplate` to replace an id in a URL fragment string:

```csharp
var data = JsonNode.Parse(
    """
    {
        "id": 12345
    }
    """
);

var sut = new StringTemplate("\"v1/test/${id}\"");
var result = sut.ToString(data);

// result will be "v1/test/12345"
```


