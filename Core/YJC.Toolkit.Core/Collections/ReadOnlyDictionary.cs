using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YJC.Toolkit.Collections
{
    //[Serializable]
    public sealed class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> fDictionary;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> source)
        {
            fDictionary = source;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (KeyValuePair<TKey, TValue> item in fDictionary)
                yield return item;
        }

        public bool ContainsKey(TKey key)
        {
            return fDictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return fDictionary.TryGetValue(key, out value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return fDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            fDictionary.CopyTo(array, arrayIndex);
        }

        public TValue this[TKey key]
        {
            get
            {
                return fDictionary[key];
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                ReadOnlyCollection<TKey> keys = new ReadOnlyCollection<TKey>(new List<TKey>(fDictionary.Keys));
                return (ICollection<TKey>)keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                ReadOnlyCollection<TValue> values = new ReadOnlyCollection<TValue>(new List<TValue>(fDictionary.Values));
                return (ICollection<TValue>)values;
            }
        }

        public int Count
        {
            get
            {
                return fDictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new NotSupportedException();
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new NotSupportedException();
        }

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get
            {
                return this[key];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
