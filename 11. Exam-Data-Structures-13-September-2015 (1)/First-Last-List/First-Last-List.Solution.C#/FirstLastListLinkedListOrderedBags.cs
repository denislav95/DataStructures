using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class FirstLastListLinkedListOrderedBags<T> : IFirstLastList<T>
    where T : IComparable<T>
{
    private OrderedBag<LinkedListNode<T>> sortedElements =
        new OrderedBag<LinkedListNode<T>>(
            (p1, p2) => p1.Value.CompareTo(p2.Value));
    private OrderedBag<LinkedListNode<T>> sortedElementsDesc =
        new OrderedBag<LinkedListNode<T>>(
            (p1, p2) => -p1.Value.CompareTo(p2.Value));

    private LinkedList<T> linkedList = new LinkedList<T>();

    public void Add(T newElement)
    {
        var linkedListNode = this.linkedList.AddLast(newElement);
        this.sortedElements.Add(linkedListNode);
        this.sortedElementsDesc.Add(linkedListNode);
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
            .Select(e => e.Value)
            .Take(count);
    }

    public IEnumerable<T> Max(int count)
    {
        if (count > this.Count)
        {
            throw new ArgumentOutOfRangeException("count");
        }

        return this.sortedElementsDesc
            .Select(e => e.Value)
            .Take(count);
    }

    public int RemoveAll(T element)
    {
        var linkedListNode = new LinkedListNode<T>(element);
        var elementsToRemove = new List<LinkedListNode<T>>(            
            this.sortedElements.Range(
            linkedListNode, true, linkedListNode, true));
        int removedCount = elementsToRemove.Count;
        foreach (var e in elementsToRemove)
        {
            this.linkedList.Remove(e);
        }
        this.sortedElements.RemoveMany(elementsToRemove);
        this.sortedElementsDesc.RemoveMany(elementsToRemove);
        return removedCount;
    }

    public void Clear()
    {
        this.sortedElements.Clear();
        this.sortedElementsDesc.Clear();
        this.linkedList.Clear();
    }
}
