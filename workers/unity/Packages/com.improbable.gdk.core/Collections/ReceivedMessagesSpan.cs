using System;

namespace Improbable.Gdk.Core
{
    public readonly ref struct ReceivedMessagesSpan<T> where T : struct, IReceivedMessage
    {
        private readonly MessageList<T> updates;

        private readonly int firstIndex;

        public readonly int Count;

        static internal ReceivedMessagesSpan<T> Empty()
        {
            return new ReceivedMessagesSpan<T>(null, 0, 0);
        }

        internal ReceivedMessagesSpan(MessageList<T> updates)
        {
            this.updates = updates;

            firstIndex = 0;
            Count = updates.Count;
        }

        internal ReceivedMessagesSpan(MessageList<T> updates, int index, int count)
        {
            this.updates = updates;
            firstIndex = index;
            Count = count;
        }

        public ref readonly T this[int index]
        {
            get
            {
                if (index >= Count)
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
