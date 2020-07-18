using System;
using System.Text.Json;
using LoveKicher.DynamicJson;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
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

            Console.WriteLine(((object)value).GetType().FullName); // DynamicJson.DynamicDictionary
        }
    }
}
