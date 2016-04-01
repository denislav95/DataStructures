namespace AvlTreeLab
{
    using System;
    
    public class AvlTree<T>
    {
        private Node<T> root;

        public int Count { get; private set; }
        
        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void ForeachDfs(Action<int, T> action)
        {
            throw new NotImplementedException();
        }
    }
}
