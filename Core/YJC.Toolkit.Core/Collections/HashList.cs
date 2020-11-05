using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace YJC.Toolkit.Collections
{
    public class HashList<T> : IEnumerable<T>, IEnumerable, ICollection<T>, ICollection, IList<T>, IList, ISet<T>
    {
        private readonly HashSet<T> fSet;
        private object fSyncRoot;

        public HashList()
        {
            fSet = new HashSet<T>();
        }

        #region IEnumerable<T> 成员

        public IEnumerator<T> GetEnumerator()
        {
            return fSet.GetEnumerator();
        }

        #endregion IEnumerable<T> 成员

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerable 成员

        #region ICollection<T> 成员

        public void Add(T item)
        {
            fSet.Add(item);
        }

        public void Clear()
        {
            fSet.Clear();
        }

        public bool Contains(T item)
        {
            return fSet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            fSet.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return fSet.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(T item)
        {
            return fSet.Remove(item);
        }

        #endregion ICollection<T> 成员

        #region ICollection 成员

        public void CopyTo(Array array, int index)
        {
            throw new NotSupportedException();
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public object SyncRoot
        {
            get
            {
                if (fSyncRoot == null)
                    Interlocked.CompareExchange(ref fSyncRoot, new object(), null);
                return fSyncRoot;
            }
        }

        #endregion ICollection 成员

        #region IList<T> 成员

        public int IndexOf(T item)
        {
            throw new NotSupportedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public T this[int index]
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion IList<T> 成员

        #region IList 成员

        public int Add(object value)
        {
            if (value is T)
            {
                fSet.Add((T)value);
                return fSet.Count - 1;
            }
            return -1;
        }

        public bool Contains(object value)
        {
            if (value is T)
                return fSet.Contains((T)value);
            else
                return false;
        }

        public int IndexOf(object value)
        {
            throw new NotSupportedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public void Remove(object value)
        {
            throw new NotSupportedException();
        }

        object IList.this[int index]
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion IList 成员

        #region ISet<T> 成员

        bool ISet<T>.Add(T item)
        {
            return fSet.Add(item);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            fSet.ExceptWith(other);
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            fSet.IntersectWith(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return fSet.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return fSet.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return fSet.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return fSet.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return fSet.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return fSet.SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            fSet.SymmetricExceptWith(other);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            fSet.UnionWith(other);
        }

        #endregion ISet<T> 成员
    }
}