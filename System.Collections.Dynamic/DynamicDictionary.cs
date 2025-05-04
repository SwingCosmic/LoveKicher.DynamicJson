using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Text;
using KV = System.Collections.Generic.Dictionary<string, object?>;
using IKV = System.Collections.Generic.IDictionary<string, object?>;

namespace System.Collections.Dynamic;

/// <summary>
/// 动态字典，允许随意读取和写入不存在的键。
/// 同时支持<c>dict.a = 1</c>或者<c>dict["a"] = 1</c>的JavaScript属性语法
/// </summary>
public class DynamicDictionary : DynamicObject, IKV
{
    private IKV dictionary;


    public DynamicDictionary() 
    {
        dictionary = new KV();
    }

    public DynamicDictionary(IKV dict)
    {
        dictionary = dict;
    }

    public static DynamicDictionary FromDictionary(IKV dict)
    {
        return new DynamicDictionary(dict);
    }


    #region DynamicObject overrides
    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        result = this[binder.Name];
        return true;
    }

    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        this[binder.Name] = value;
        return true;
    }

    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object? result)
    {
        if (indexes.Length == 1)
        {
            if (indexes[0] is string key)
            {
                result = this[key];
                return true;
            }
        }
        result = null;
        return false;
    }

    public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object? value)
    {
        if (indexes.Length == 1)
        {
            if (indexes[0] is string key)
            {
                this[key] = value;
                return true;
            }
        }
        return false;
    } 
    #endregion        
    
    
    #region Dictionary overrides

    public object? this[string key] 
    {
        get
        {
            if (dictionary.TryGetValue(key, out object? value))
                return value;
            else
                return null;
        } 
        set => dictionary[key] = value; 
    }

    public ICollection<string> Keys => dictionary.Keys;

    public ICollection<object?> Values => dictionary.Values;

    public int Count => dictionary.Count;

    public bool IsReadOnly => false;

    public void Add(string key, object? value)
    {
        dictionary.Add(key, value);
    }

    public void Add(KeyValuePair<string, object?> item)
    {
        dictionary.Add(item);
    }

    public void Clear()
    {
        dictionary.Clear();
    }

    public bool Contains(KeyValuePair<string, object?> item)
    {
        return dictionary.Contains(item);
    }

    public bool ContainsKey(string key)
    {
        return dictionary.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
    {
        dictionary.CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    public bool Remove(string key)
    {
        return dictionary.Remove(key);
    }

    public bool Remove(KeyValuePair<string, object?> item)
    {
        return dictionary.Remove(item);
    }

    public bool TryGetValue(string key, out object? value)
    {
        var res = dictionary.TryGetValue(key, out value);
        if (res == false) value = null;
        return true;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    #endregion
}
