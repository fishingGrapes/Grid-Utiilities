using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Simple Scriptable Object Wrapper around a Generic C# List 
/// </summary>
public class RuntimeList<T> : ScriptableObject
{
    private List<T> items = new List<T>();

    public void Add(T t)
    {
        items.Add(t);
    }

    public void Remove(T t)
    {
        items.Remove(t);
    }

    public T Remove(int index)
    {
        T t = items[index];
        items.RemoveAt(index);

        return t;
    }

    public void Clear()
    {
        items.Clear();
    }

    public T Get(int index)
    {
        return items[index];
    }

    public bool Contains(T t)
    {
        return items.Contains(t);
    }

    public List<T> GetAll()
    {
        return items;
    }

    public T[] GetArray()
    {
        return items.ToArray();
    }

    public int Count { get { return items.Count; } }
}
