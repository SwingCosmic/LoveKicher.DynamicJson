using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace System.Collections.Dynamic;

/// <summary>
/// 灵活列表，允许读写负值下标、读取越界的下标和调整<see cref="Count"/>属性。
/// 通常情况下<typeparamref name="T"/>应该为引用类型或者<see cref="Nullable{T}"/>
/// </summary>
public class FlexibleList<T> : IList<T?> 
{
    private readonly List<T?> list;

    public FlexibleList()
    {
        list = new ();
    }

    public FlexibleList(int capacity)
    {
        list = new (capacity);
    }

    public FlexibleList(IList<T?> list)
    {
        this.list = list.ToList();
    }

    /// <summary>
    /// 调整列表的大小
    /// </summary>
    /// <param name="size">调整后的大小。
    /// <list type="bullet">
    ///   <item>如果<paramref name="size"/>大于<see cref="Count"/>，则会填充<see langword="null"/>直到等于<paramref name="size"/></item>
    ///   <item>如果<paramref name="size"/>小于<see cref="Count"/>，则会尝试截断末尾的元素直到等于<paramref name="size"/></item>
    /// </list>
    /// </param>
    /// <returns>调整后的列表大小</returns>
    /// <exception cref="ArgumentOutOfRangeException">大小为负数，或者超过了列表长度</exception>
    public void Resize(int size)
    {
        if (size < 0) 
            throw new ArgumentOutOfRangeException(nameof(size));

        if (size == 0)
        {
            list.Clear();
            return;
        }

        if (size == list.Count)
            return;
        else if (size > list.Count)
            list.AddRange(Enumerable.Repeat<T?>(default, size - list.Count));
        else
            list.RemoveRange(size, list.Count - size);
    }

    #region List overrides
    public T? this[int index] 
    { 
        get 
        {
            if (index < 0)
            {
                if (-index <= list.Count)
                    return list[list.Count + index];
                else
                    return default;
            } 
            else if (index < list.Count)
                return list[index];
            return default;
        }
        set
        {
            // May cause ArgumentOutOfRangeException
            if (index < 0)
                list[list.Count + index] = value;
            else
                list[index] = value;
        }
    }

    public int Count
    {
        get => list.Count;
        set => Resize(value);
    }

    public bool IsReadOnly => false;

    public void Add(T? item) => list.Add(item);

    public void Clear() => list.Clear();

    public bool Contains(T? item) => list.Contains(item);

    public void CopyTo(T?[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

    public IEnumerator<T?> GetEnumerator() => list.GetEnumerator();

    public int IndexOf(T? item) => list.IndexOf(item);

    public void Insert(int index, T? item) => list.Insert(index, item);

    public bool Remove(T? item) => list.Remove(item);

    public void RemoveAt(int index)
    {
        if (index < 0)
        {
            if (-index <= list.Count)
                list.RemoveAt(list.Count + index);
        }
        else if (index < list.Count)
            list.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
    #endregion
}
