using System.Collections;
using System.Collections.Generic;

namespace Improbable.Gdk.Core.Collections
{
    public struct BlittableMap<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly uint handle;

        internal Dictionary<TKey, TValue> Internal =>
            CollectionProvider<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>.Get(handle);

        internal BlittableMap(uint handle)
        {
            this.handle = handle;
        }

        #region IDictionary

        public void Clear()
        {
            Internal.Clear();
        }

        public int Count => Internal.Count;

        public void Add(TKey key, TValue value)
        {
            Internal.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return Internal.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return Internal.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return Internal.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get => Internal[key];
            set => Internal[key] = value;
        }

        public static implicit operator Dictionary<TKey, TValue>(BlittableMap<TKey, TValue> bl)
        {
            return bl.Internal;
        }

        public ICollection<TKey> Keys => Internal.Keys;
        public ICollection<TValue> Values => Internal.Values;

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            return Internal.GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>) Internal).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) Internal).GetEnumerator();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>) Internal).Add(item);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>) Internal).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>) Internal).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>) Internal).Remove(item);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly =>
            ((ICollection<KeyValuePair<TKey, TValue>>) Internal).IsReadOnly;

        #endregion
    }
}
