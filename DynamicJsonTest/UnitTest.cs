using LoveKicher.DynamicJson;
using NUnit.Framework;
using System;
using System.Collections.Dynamic;
using System.Text.Json;

namespace DynamicJsonTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void DynamicDictionaryTest()
        {
            dynamic obj = new DynamicDictionary();
            obj["111"] = 666;
            obj.abc = new[] { 1, 2, 3 };

            var json = JsonSerializer.Serialize(obj);
            Assert.That(@"{""111"":666,""abc"":[1,2,3]}", Is.EqualTo(json));

            DynamicDictionary obj2 = JsonSerializer.Deserialize<DynamicDictionary>(json);
            Assert.That(obj2, Has.Count.EqualTo(2));
        }

        [Test]
        public void DeserializeDynamicTest()
        {
            dynamic obj = new DynamicDictionary
            {
                ["a"] = double.MaxValue,
                ["b"] = null,
                ["x"] = new dynamic[]
                {
                    true , "666", new DynamicDictionary
                    {
                        ["？`%$xcdd4"] = "foo",
                        ["愛`エ口$区♂《"] = new [] { 1, 1, 4 }
                    }
                }
            };
            var json1 = JsonSerializer.Serialize(obj);
            JsonElement e = JsonSerializer.Deserialize<object>(json1);
            dynamic obj2 = e.DeserializeDynamic();
            var json2 = JsonSerializer.Serialize(obj2);

            Assert.DoesNotThrow(() => { obj2.bar = obj2.x[2]["./;gdl/\tlkfd ~`y7"]; });
            Assert.DoesNotThrow(() => { obj2.x[1] = null; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { obj2.x[4] = "error"; });
            Assert.That(json1, Is.EqualTo(json2));
        }
    }
}