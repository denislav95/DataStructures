using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class FirstLastListDictionaries<T> : IFirstLastList<T>
    where T : IComparable<T>
{
    private OrderedMultiDictionary<T, int> elements =
        new OrderedMultiDictionary<T, int>(true);
    private OrderedDictionary<int, T> elementsByOrder =
        new OrderedDictionary<int, T>();
    private int addCounter = 0;

    public void Add(T newElement)
    {
        ++addCounter;
        this.elements.Add(newElement, addCounter);
        this.elementsByOrder.Add(addCounter, newElement);
    }

    public int Count
    {
        get
        {
            return this.elementsByOrder.Count;
        }
    }

    public IEnumerable<T> First(int count)
    {
        if (count > this.Count)
        {
            throw new ArgumentOutOfRangeException("count");
        }

        return this.elementsByOrder.Select(e => e.Value).Take(count);
    }

    public IEnumerable<T> Last(int count)
    {
        if (count > this.Count)
        {
            throw new ArgumentOutOfRangeException("count");
        }

        return this.elementsByOrder.Reversed()
            .Select(e => e.Value).Take(count);
    }

    public IEnumerable<T> Min(int count)
    {
        if (count > this.Count)
        {
            throw new ArgumentOutOfRangeException("count");
        }
        var values = this.elements.SelectMany(e => e.Value).Take(count);
        var items = values.Select(v => this.elementsByOrder[v]);
        return items;
    }

    public IEnumerable<T> Max(int count)
    {
        if (count > this.Count)
        {
            throw new ArgumentOutOfRangeException("count");
        }

        var values = this.elements.Reversed()
            .SelectMany(e => e.Value).Take(count);
        var items = values.Select(v => this.elementsByOrder[v]);
        return items;
    }

    public int RemoveAll(T element)
    {
        var itemsForRemoval = this.elements[element];
        var removedCount = itemsForRemoval.Count;
        this.elementsByOrder.RemoveMany(itemsForRemoval);
        this.elements.Remove(element);
        return removedCount;
    }

    public void Clear()
    {
        this.elements.Clear();
        this.elementsByOrder.Clear();
    }
}
