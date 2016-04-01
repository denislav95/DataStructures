namespace First_Last_List
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wintellect.PowerCollections;

    public class FirstLastList<T> : IFirstLastList<T>
        where T : IComparable<T>
    {
        private readonly LinkedList<T> _elements = new LinkedList<T>();

        private readonly OrderedDictionary<T, List<LinkedListNode<T>>> _orderedElements =
            new OrderedDictionary<T, List<LinkedListNode<T>>>();

        public void Add(T newElement)
        {
            var elementNode = this._elements.AddLast(newElement);

            if (!this._orderedElements.ContainsKey(newElement))
            {
                this._orderedElements[newElement] = new List<LinkedListNode<T>>();
            }

            this._orderedElements[newElement].Add(elementNode);
        }

        public int Count
        {
            get { return this._elements.Count; }
        }

        public IEnumerable<T> First(int count)
        {
            this.ValidateCount(count);

            return this._elements.Take(count);
        }

        public IEnumerable<T> Last(int count)
        {
            this.ValidateCount(count);

            return this._elements.Reverse().Take(count);
        }

        public IEnumerable<T> Min(int count)
        {
            this.ValidateCount(count);

            return this._orderedElements
                .SelectMany(e => e.Value)
                .Select(e => e.Value)
                .Take(count);
        }

        public IEnumerable<T> Max(int count)
        {
            this.ValidateCount(count);

            return this._orderedElements
                .Reversed()
                .SelectMany(e => e.Value)
                .Select(e => e.Value)
                .Take(count);
        }

        public int RemoveAll(T element)
        {
            if (!this._orderedElements.ContainsKey(element))
            {
                return 0;
            }

            var elementNode = this._orderedElements[element];

            foreach (var linkedListNode in elementNode)
            {
                this._elements.Remove(linkedListNode);
            }

            var removedElements = elementNode.Count;

            this._orderedElements.Remove(element);

            return removedElements;
        }

        public void Clear()
        {
            this._elements.Clear();
            this._orderedElements.Clear();
        }

        public void ValidateCount(int count)
        {
            if (this.Count < count)
            {
                throw new ArgumentOutOfRangeException("count");
            }
        }
    }
}
