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
			_heap = new T[((int)Math.Pow(2, Math.Ceiling(Math.Log(minSize, 2))))];
		}

		/// <summary>
		/// Add a new value to the Heap.
		/// </summary>
		/// <param name="item"></param>
		public void Push(T item)
		{
			if(_indexDictionary.TryGetValue(item, out int index))
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
				ShiftUp(Count);
				Count++;
            }
		}

		/// <summary>
		/// View the value currently at the top of the Heap.
		/// </summary>
		/// <returns></returns>
		public T Peek()
		{
			if(_heap.Length == 0) throw new ArgumentOutOfRangeException("No values in heap");
			return _heap[0];
		}

		/// <summary>
		/// Remove the value currently at the top of the Heap and return it.
		/// </summary>
		/// <returns></returns>
		public T Pop()
		{
			T item = Peek();
			Count--;
			_heap[0] = _heap[Count];
			ShiftDown(0);
			_indexDictionary.Remove(item);
			return item;
		}

		/// <summary>
		/// Move an element up the Heap.
		/// </summary>
		/// <param name="heapIndex"></param>
		private void ShiftUp(int heapIndex)
		{
			if(heapIndex == 0) return;
			int parentIndex = (heapIndex - 1) / 2;
			bool shouldShift = DoCompare(parentIndex, heapIndex) > 0;
			if(!shouldShift) return;
			Swap(parentIndex, heapIndex);
			ShiftUp(parentIndex);
		}

		/// <summary>
		/// Move an element down the Heap.
		/// </summary>
		/// <param name="heapIndex"></param>
		private void ShiftDown(int heapIndex)
		{
			int child1 = heapIndex * 2 + 1;
			if(child1 >= Count) return;
			int child2 = child1 + 1;

			int preferredChildIndex = (child2 >= Count || DoCompare(child1, child2) <= 0) ? child1 : child2;
			if(DoCompare(preferredChildIndex, heapIndex) > 0) return;
			Swap(heapIndex, preferredChildIndex);
			ShiftDown(preferredChildIndex);
		}

		/// <summary>
		/// Swap two items in the underlying array.
		/// </summary>
		/// <param name="index1"></param>
		/// <param name="index2"></param>
		private void Swap(int index1, int index2)
		{
			(_heap[index1], _heap[index2]) = (_heap[index2], _heap[index1]);

			_indexDictionary[_heap[index1]] = index1;
			_indexDictionary[_heap[index2]] = index2;
		}

		/// <summary>
		/// Perform a comparison between two elements of the heap.
		/// This method encapsulates the min/max behavior of the heap so the rest of the class can remain blissfully ignorant.
		/// </summary>
		/// <param name="initialIndex"></param>
		/// <param name="contenderIndex"></param>
		/// <returns></returns>
		private int DoCompare(int initialIndex, int contenderIndex)
		{
			T initial = _heap[initialIndex];
			T contender = _heap[contenderIndex];
			int value = _comparer.Compare(initial, contender);
			return _isMaxHeap ? -value : value;
		}

		public void Clear()
		{
			_indexDictionary.Clear();
			Count = 0;
		}
    }
}
