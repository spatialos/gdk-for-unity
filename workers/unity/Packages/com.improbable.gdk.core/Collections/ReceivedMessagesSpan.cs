using System;

namespace Improbable.Gdk.Core
{
    public readonly ref struct ReceivedMessagesSpan<T> where T : struct, IReceivedMessage
    {
        private readonly ReceivedMessageList<T> updates;

        private readonly int firstIndex;

        public int Count { get; }

        internal ReceivedMessagesSpan(ReceivedMessageList<T> updates)
        {
            this.updates = updates;

            firstIndex = 0;
            Count = updates.Count;
        }

        internal ReceivedMessagesSpan(ReceivedMessageList<T> updates, int index, int count)
        {
            this.updates = updates;
            firstIndex = index;
            Count = count;
        }

        public ref readonly T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException();
                }

                return ref updates[firstIndex + index];
            }
        }

        public T[] ToArray()
        {
            var array = new T[Count];
            for (int i = 0; i < Count; ++i)
            {
                array[i] = this[i];
            }

            return array;
        }
    }
}
