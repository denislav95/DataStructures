using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class FirstLastListLinkedList<T> : IFirstLastList<T>
    where T : IComparable<T>
{
    private OrderedMultiDictionary<T, LinkedListNode<T>> sortedElements =
        new OrderedMultiDictionary<T, LinkedListNode<T>>(
            true,
            (p1, p2) => p1.CompareTo(p2),
            (p1, p2) => p1.Value.CompareTo(p2.Value));

    private LinkedList<T> linkedList = new LinkedList<T>();

    public void Add(T newElement)
    {
        var linkedListNode = this.linkedList.AddLast(newElement);
        this.sortedElements.Add(newElement, linkedListNode);
    }

    public int Count
    {
        get
        {
            return this.linkedList.Count;
        }
    }

    public IEnumerable<T> First(int count)
    {
        if (count > this.Count)
        {
            throw new ArgumentOutOfRangeException("count");
        }

        return this.linkedList.Take(count);
    }

    public IEnumerable<T> Last(int count)
    {
        if (count > this.Count)
        {
            throw new ArgumentOutOfRangeException("count");
        }

        var node = this.linkedList.Last;
        while (count > 0 && node != null)
        {
            count--;
            yield return node.Value;
            node = node.Previous;
        }
    }

    public IEnumerable<T> Min(int count)
    {
        if (count > this.Count)
        {
            throw new ArgumentOutOfRangeException("count");
        }

        return this.sortedElements
            .SelectMany(e => e.Value)
            .Select(e => e.Value)
            .Take(count);
    }

    public IEnumerable<T> Max(int count)
    {
        if (count > this.Count)
        {
            throw new ArgumentOutOfRangeException("count");
        }

        return this.sortedElements
            .Reversed()
            .SelectMany(e => e.Value)
            .Select(e => e.Value)
            .Take(count);
    }

    public int RemoveAll(T element)
    {
        var elementsToRemove = this.sortedElements[element];
        int removedCount = elementsToRemove.Count;
        foreach (var e in elementsToRemove)
        {
            this.linkedList.Remove(e);
        }
        this.sortedElements.Remove(element);
        return removedCount;
    }

    public void Clear()
    {
        this.sortedElements.Clear();
        this.linkedList.Clear();
    }
}
