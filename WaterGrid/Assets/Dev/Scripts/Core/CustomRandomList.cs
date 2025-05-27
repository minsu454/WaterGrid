using System;
using System.Collections.Generic;
using System.Diagnostics;

public sealed class CustomRandomList<TValue>
{
    private readonly SortedList<int, List<TValue>> _list = new();
    private int totalWeight = 0;

    public void Add(int key, TValue value)
    {
        if (key <= 0)
            return;

        if (ContainsKey(key) is true)
        {
            _list[key].Add(value);
            return;
        }

        totalWeight += key;
        _list.Add(key, new List<TValue>() { value });
    }

    public void Remove(int key, TValue value)
    {
        if (TryGetValue(key, out List<TValue> valueList) is false)
            return;

        if (valueList.Contains(value) is false)
            return;

        valueList.Remove(value);

        if (valueList.Count == 0)
        {
            totalWeight -= key;
            _list.Remove(key);
        }
        else
        {
            _list[key] = valueList;
        }
    }

    public bool ContainsKey(int key)
    {
        return _list.ContainsKey(key);
    }

    public bool TryGetValue(int key, out List<TValue> value)
    {
        return _list.TryGetValue(key, out value);
    }

    public TValue RandomPick()
    {
        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float weight = 0;

        foreach (var item in _list)
        {
            weight += item.Key;

            if (randomValue < weight)
            {
                return item.Value[UnityEngine.Random.Range(0, item.Value.Count)];
            }
        }

        throw new InvalidOperationException("RandomPick failed unexpectedly");
    }

    public void Clear()
    {
        totalWeight = 0;
        _list.Clear();
    }
}