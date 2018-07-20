using System.Collections;
using System.Collections.Generic;

namespace Improbable.Gdk.Core.Collections
{
    public struct BlittableList<TValue> : IList<TValue>
    {
        private readonly uint handle;

        internal List<TValue> Internal => CollectionProvider<List<TValue>, TValue>.Get(handle);

        internal BlittableList(uint handle)
        {
            this.handle = handle;
        }

        public static implicit operator List<TValue>(BlittableList<TValue> bl)
        {
            return bl.Internal;
        }

        #region IList<T>

        public TValue this[int index]
        {
            get => Internal[index];
            set => Internal[index] = value;
        }

        public int Count => Internal.Count;

        public bool IsReadOnly => ((ICollection<TValue>) Internal).IsReadOnly;

        public void Add(TValue item)
        {
            Internal.Add(item);
        }

        public void Insert(int index, TValue item)
        {
            Internal.Insert(index, item);
        }

        public void Clear()
        {
            Internal.Clear();
        }

        public bool Contains(TValue item)
        {
            return Internal.Contains(item);
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            Internal.CopyTo(array, arrayIndex);
        }

        public int IndexOf(TValue item)
        {
            return Internal.IndexOf(item);
        }

        public bool Remove(TValue item)
        {
            return Internal.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Internal.RemoveAt(index);
        }

        public List<TValue>.Enumerator GetEnumerator()
        {
            return Internal.GetEnumerator();
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return ((IEnumerable<TValue>) Internal).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) Internal).GetEnumerator();
        }

        #endregion
    }
}
