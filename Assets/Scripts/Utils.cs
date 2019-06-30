using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIndexable<T>
{
    T this[int key]
    {
        get;
        set;
    }
}

[Serializable]
public class ListWrapper<T> : IIndexable<T>
{
    public List<T> list;

    public T this[int key]
    {
        get
        {
            return list[key];
        }
        set
        {
            list[key] = value;
        }
    }
}

[Serializable]
public class ListInt : ListWrapper<int> { }
[Serializable]
public class ListFloat : ListWrapper<float> { }
[Serializable]
public class ListString : ListWrapper<string> { }
[Serializable]
public class ListVector2Int : ListWrapper<Vector2Int> { }

public static class Utils
{
    public static bool ListToDictionary<K, V>(List<K> keys, List<V> values, ref Dictionary<K, V> dictionary)
    {
        if(dictionary == null || keys.Count != values.Count)
        {
            return false;
        }

        for (int i = 0; i < keys.Count; i++)
        {
            K key = keys[i];
            V value = values[i];
            dictionary[key] = value;
        }
        return true;
    }

    public static bool ListToDictionaryList<K, W, V>(List<K> keys, List<W> valueWrappers, ref Dictionary<K, List<V>> dictionary) where W : ListWrapper<V>
    {
        if (dictionary == null || keys.Count != valueWrappers.Count)
        {
            return false;
        }

        for (int i = 0; i < keys.Count; i++)
        {
            K key = keys[i];
            W listWrapper = valueWrappers[i];
            List<V> value = listWrapper.list;
            dictionary[key] = value;
        }
        return true;
    }
}
