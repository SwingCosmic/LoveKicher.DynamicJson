using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace DynamicJson
{
    public static class JsonExtension
    {

        static JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Convert <see cref="JsonElement"/> to JavaScript-like dyanmic object.
        /// </summary>
        /// <param name="element">The <see cref="JsonElement"/> to be converted.</param>
        /// <returns>A dynamic object like the result of <c>JSON.parse</c>, 
        /// can be <see cref="String"/>, <see cref="Double"/>, <see cref="Boolean"/>,
        /// <see langword="null"/>, <see cref="DynamicDictionary"/> or <see cref="IList{dyanmic}"/>.
        /// </returns>
        public static dynamic DeserializeDynamic(this JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.True || element.ValueKind == JsonValueKind.False)
            {
                return element.GetBoolean();
            }
            else if (element.ValueKind == JsonValueKind.Number)
            {
                return element.GetDouble();
            }
            else if (element.ValueKind == JsonValueKind.String)
            {
                return element.GetString();
            }
            else if (element.ValueKind == JsonValueKind.Object)
            {
                return element
                    .EnumerateObject()
                    .Aggregate(new DynamicDictionary(), (a, v) =>
                    {
                        a[v.Name] = DeserializeDynamic(v.Value);
                        return a;
                    });
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                return element
                    .EnumerateArray()
                    .Select(v => DeserializeDynamic(v))
                    .ToList();
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Convert .NET object to JavaScript-like dynamic object.
        /// </summary>
        /// <param name="obj">The object to be converted.</param>
        /// <param name="camelCase">Whether to convert .NET properties(usually PascalCase) to camelCase, 
        /// default value is <see langword="false"/>.</param>
        /// <returns></returns>
        public static dynamic AsDynamic(this object obj, bool camelCase = false)
        {
            return JsonSerializer.Deserialize<JsonElement>
            (
                JsonSerializer.Serialize
                (
                    obj, camelCase ? serializeOptions : null
                )
            ).DeserializeDynamic();
        }
    
    }
}
