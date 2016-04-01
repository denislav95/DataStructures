namespace QuadTree.Core
{
    using System;
    using System.Collections.Generic;

    public class QuadTree<T> where T : IBoundable
    {
        public const int DefaultMaxDepth = 5;

        public readonly int MaxDepth;

        private Node<T> root;

        public QuadTree(int width, int height, int maxDepth = DefaultMaxDepth)
        {
            // TODO:
            this.Bounds = this.root.Bounds;
            this.MaxDepth = maxDepth;
        }

        public int Count { get; private set; }

        public Rectangle Bounds { get; private set; }

        public bool Insert(T item)
        {
            throw new NotImplementedException();
        }

        public List<T> Report(Rectangle bounds)
        {
            throw new NotImplementedException();
        }

        public void ForEachDfs(Action<List<T>, int, int> action)
        {
            throw new NotImplementedException();
        }
    }
}
