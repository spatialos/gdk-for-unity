using System;

namespace Improbable.Gdk.Core
{
    public readonly ref struct MessagesSpan<T> where T : struct
    {
        private readonly MessageList<T> source;

        private readonly int firstIndex;

        public readonly int Count;

        internal static MessagesSpan<T> Empty()
        {
            return new MessagesSpan<T>(null, 0, 0);
        }

        internal MessagesSpan(MessageList<T> source, int index, int count)
        {
            this.source = source;
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

                return ref source[firstIndex + index];
            }
        }
    }
}
