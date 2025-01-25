using System;
using System.Collections.Generic;

namespace Bas.Catan.Collections
{
    /// <summary>
    /// Array-based Heap implementation, with the ability to remove items.
    /// Original code -> https://gist.github.com/roufamatic/ee7e11469809f2b276c0d3dc6b8dd80b
    /// </summary>
    /// <typeparam name="T">Kind of thing being stored in the heap.</typeparam>
    public class Heap<T>
    {
        public int Count { get; private set; }

        private readonly Dictionary<T, int> _indexDictionary;
        private readonly IComparer<T> _comparer;
        private readonly bool _isMaxHeap;
        private T[] _heap;

        /// <summary>
        /// Create a new heap.
        /// </summary>
        /// <param name="minSize">The minimum number of elements the heap is expected to hold.</param>
        /// <param name="comparer">Comparer</param>
        /// <param name="isMaxHeap">If "true", this is a Max Heap, where the largest values rise to the top. Otherwise, this is a Min Heap.</param>
        public Heap(int minSize = 1024, IComparer<T> comparer = null, bool isMaxHeap = false)
        {
            _indexDictionary = new Dictionary<T, int>();
            _comparer = comparer ?? Comparer<T>.Default;
            _isMaxHeap = isMaxHeap;
            _heap = new T[(int) Math.Pow(2, Math.Ceiling(Math.Log(minSize, 2)))];
        }

        /// <summary>
        /// Add a new value to the Heap.
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item)
        {
            if (_indexDictionary.TryGetValue(item, out int index))
            {
                ShiftUp(index);
            }
            else
            {
                if (Count == _heap.Length)
                {
                    Array.Resize(ref _heap, _heap.Length * 2);
                }
                _heap[Count] = item;
                _indexDictionary[item] = Count;
                ShiftUp(Count++);
            }
        }

        /// <summary>
        /// Remove the value currently at the top of the Heap and return it.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T item = _heap[0];
            _heap[0] = _heap[--Count];
            _indexDictionary[_heap[0]] = 0;
            _indexDictionary.Remove(item);
            ShiftDown(0);
            return item;
        }

        /// <summary>
        /// Move an element up the Heap.
        /// </summary>
        /// <param name="heapIndex"></param>
        private void ShiftUp(int heapIndex)
        {
            T item = _heap[heapIndex];
            while (heapIndex > 0)
            {
                int parentIndex = (heapIndex - 1) / 2;
                if (Compare(item, _heap[parentIndex]) >= 0)
                {
                    break;
                }
                _heap[heapIndex] = _heap[parentIndex];
                _indexDictionary[_heap[heapIndex]] = heapIndex;
                heapIndex = parentIndex;
            }
            _heap[heapIndex] = item;
            _indexDictionary[item] = heapIndex;
        }

        /// <summary>
        /// Move an element down the Heap.
        /// </summary>
        /// <param name="heapIndex"></param>
        private void ShiftDown(int heapIndex)
        {
            T item = _heap[heapIndex];
            int count = Count;
            while (heapIndex < count)
            {
                int child1 = heapIndex * 2 + 1;
                if (child1 >= count) break;
                int child2 = child1 + 1;
                int preferredChildIndex = (child2 >= count || Compare(_heap[child1], _heap[child2]) <= 0) ? child1 : child2;
                if (Compare(item, _heap[preferredChildIndex]) <= 0) break;
                _heap[heapIndex] = _heap[preferredChildIndex];
                _indexDictionary[_heap[heapIndex]] = heapIndex;
                heapIndex = preferredChildIndex;
            }
            _heap[heapIndex] = item;
            _indexDictionary[item] = heapIndex;
        }

        /// <summary>
        /// Perform a comparison between two elements of the heap.
        /// This method encapsulates the min/max behavior of the heap so the rest of the class can remain blissfully ignorant.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int Compare(T x, T y)
        {
            int value = _comparer.Compare(x, y);
            return _isMaxHeap ? -value : value;
        }

        public void Clear()
        {
            _indexDictionary.Clear();
            Count = 0;
        }
    }
}