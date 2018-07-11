using System.Collections;
using System.Collections.Generic;

namespace Assets.Gdk.Core.Collections
{
    public struct BlittableList<T> : IList<T>
    {
        private readonly uint handle;

        internal List<T> Internal => ListProvider<T>.Get(handle);

        internal BlittableList(uint handle)
        {
            this.handle = handle;
        }

        public static implicit operator List<T>(BlittableList<T> bl) => bl.Internal;

        #region IList<T>
        public T this[int index] {
            get { return Internal[index]; }
            set { Internal[index] = value; }
        }

        public int Count => Internal.Count;

        public bool IsReadOnly => ((ICollection<T>)Internal).IsReadOnly;

        public void Add(T item) => Internal.Add(item);

        public void Insert(int index, T item) => Internal.Insert(index, item);

        public void Clear() => Internal.Clear();

        public bool Contains(T item) => Internal.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => Internal.CopyTo(array, arrayIndex);

        public int IndexOf(T item) => Internal.IndexOf(item);

        public bool Remove(T item) => Internal.Remove(item);

        public void RemoveAt(int index) => Internal.RemoveAt(index);

        public List<T>.Enumerator GetEnumerator() => Internal.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)Internal).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Internal).GetEnumerator();
        #endregion
    }
}
