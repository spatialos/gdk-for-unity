using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    /// <summary>
    /// Priority queue implemented with a max heap
    /// </summary>
    /// <typeparam name="T"> Comparable data type </typeparam>
    public class PriorityQueue<T>
    {
        private readonly List<T> heap = new List<T>();
        private readonly IComparer<T> comparer;

        public PriorityQueue()
        {
            comparer = Comparer<T>.Default;
        }

        public PriorityQueue(IComparer<T> comparer) : this()
        {
            if (comparer != null)
            {
                this.comparer = comparer;
            }
        }

        public int Count()
        {
            return heap.Count;
        }

        public T Peak()
        {
            if (heap.Count == 0)
            {
                throw new Exception($"Tried to peak value from empty {typeof(PriorityQueue<T>).Name}");
            }
            return heap[0];
        }

        public T Pop()
        {
            if (heap.Count == 0)
            {
                throw new Exception($"Tried to pop value from empty {typeof(PriorityQueue<T>).Name}");
            }

            T head = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            var currentIndex = 0;

            while (TrySwapWithChild(ref currentIndex))
            {
            }

            return head;
        }

        public void Push(T element)
        {
            heap.Add(element);
            var currentIndex = heap.Count - 1;
            while (TrySwapWithParent(ref currentIndex))
            {
            }
        }

        private bool TrySwapWithChild(ref int currentIndex)
        {
            var leftChildIndex = (currentIndex * 2) + 1;
            var rightChildIndex = leftChildIndex + 1;

            // Get the index of the largest child, or return false if there are no children
            int indexOfLargestChild;
            if (heap.Count > rightChildIndex)
            {
                indexOfLargestChild = comparer.Compare(heap[rightChildIndex], heap[leftChildIndex]) > 0
                    ? rightChildIndex
                    : leftChildIndex;
            }
            else if (heap.Count > leftChildIndex)
            {
                indexOfLargestChild = leftChildIndex;
            }
            else
            {
                return false;
            }

            // Swap if the largest child is larger than the parent
            if (comparer.Compare(heap[indexOfLargestChild], heap[currentIndex]) > 0)
            {
                T temp = heap[currentIndex];
                heap[currentIndex] = heap[indexOfLargestChild];
                heap[indexOfLargestChild] = temp;
                currentIndex = indexOfLargestChild;
                return true;
            }
            return false;
        }

        private bool TrySwapWithParent(ref int currentIndex)
        {
            if (currentIndex == 0)
            {
                return false;
            }
            var parentIndex = currentIndex % 2 == 0
                ? (currentIndex - 2) / 2
                : (currentIndex - 1) % 2;

            if (comparer.Compare(heap[currentIndex], heap[parentIndex]) > 0)
            {
                T temp = heap[parentIndex];
                heap[parentIndex] = heap[currentIndex];
                heap[currentIndex] = temp;
                currentIndex = parentIndex;
                return true;
            }
            return false;
        }
    }
}
