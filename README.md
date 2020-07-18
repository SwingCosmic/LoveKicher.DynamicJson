# DynamicJson
Deserialize JSON to JavaScript-like dynamic object.

![Nuget](https://img.shields.io/nuget/v/LoveKicher.DynamicJson?color=green)
![Release](https://img.shields.io/github/v/release/SwingCosmic/LoveKicher.DynamicJson)
## Requirement
* System.Text.Json

## Quick Start

```csharp
using System.Text.Json;
using LoveKicher.DynamicJson;

var json =
@"{
    'numberValue': 9876547210.33,
    'boolValue': false,
    'array': [
        null, 
        'someText', 
        {
            'nested.prop': 'nested value'
        }
    ]
}".Replace("'", "\"");

dynamic value = JsonSerializer
    .Deserialize<JsonElement>(json)
    .DeserializeDynamic();

Console.WriteLine(value.boolValue); // false
Console.WriteLine(value.array[2]["nested.prop"]); // nested value

Console.WriteLine(((object)value).GetType().FullName); // LoveKicher.DynamicJson.DynamicDictionary

```
