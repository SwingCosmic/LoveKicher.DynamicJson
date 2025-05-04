namespace DynamicJsonTest;

using NUnit.Framework;
using System;
using System.Collections.Dynamic;
using System.Linq;

public class DynamicTest
{
    [Test]
    public void DynamicDictionary()
    {
        dynamic obj = new DynamicDictionary();
        obj["abc"] = 666;
        obj.abc = 888;

        Assert.That(obj.abc, Is.EqualTo(888));

        Assert.DoesNotThrow(() => { var a = obj["ttttt"]; });
    }

    [Test]
    public void DynamicList()
    {
        var l = new FlexibleList<int?>([1, 2, 3, 4, 5]);
        Assert.Multiple(() =>
        {
            Assert.That(l[0], Is.EqualTo(1));
            Assert.That(l[-1], Is.EqualTo(5));
            Assert.That(l[-6], Is.EqualTo(null));
            Assert.That(l[5], Is.EqualTo(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => l[10] = 9);
        });

        l.Count = 3;
        Assert.That(l.ToArray(), Is.EqualTo(new [] { 1, 2, 3 }));
        
        l.Count = 5;
        Assert.That(l.ToArray(), Is.EqualTo(new int?[] { 1, 2, 3, null, null }));
    }
}