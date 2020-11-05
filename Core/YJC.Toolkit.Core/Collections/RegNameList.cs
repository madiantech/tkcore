using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Collections
{
    public class RegNameList<T> : IList<T>, IList, ICollection<T>, ICollection,
        IEnumerable<T>, IEnumerable where T : class, IRegName
    {
        private readonly LinkList<T> fList;
        private readonly Dictionary<string, ListNode<T>> fDictionary;
        private object fSyncRoot;

        public RegNameList()
        {
            fList = new LinkList<T>();
            fDictionary = new Dictionary<string, ListNode<T>>();
        }

        #region IList<T> 成员

        public int IndexOf(T item)
        {
            //TkDebug.AssertArgumentNull(item, "item", this);
            if (item == null)
                return -1;

            ListNode<T> node;
            if (fDictionary.TryGetValue(item.RegName, out node))
                return fList.IndexOf(node);
            return -1;
        }

        public void Insert(int index, T item)
        {
            AssertIndex(index, Count);
            TkDebug.AssertArgumentNull(item, "item", this);

            if (!fDictionary.ContainsKey(item.RegName))
            {
                ListNode<T> node = fList.GetIndexNode(index);
                var newNode = fList.AddAfter(node, item);
                fDictionary.Add(item.RegName, newNode);
                checked
                {
                    OnAdded(item, index + 1);
                }
            }
        }

        public void RemoveAt(int index)
        {
            AssertIndex(index, Count);

            ListNode<T> node = fList.GetIndexNode(index);
            fDictionary.Remove(node.Value.RegName);
            fList.Remove(node);
            OnRemoved(node.Value, index);
            node = null;
        }

        public T this[int index]
        {
            get
            {
                AssertIndex(index, Count);

                return fList[index];
            }
            set
            {
                AssertIndex(index, Count);

                fList[index] = value;
            }
        }

        #endregion IList<T> 成员

        #region ICollection<T> 成员

        public void Add(T item)
        {
            TkDebug.AssertArgumentNull(item, "item", this);

            InternalAdd(item);
        }

        public void Clear()
        {
            fDictionary.Clear();
            fList.Clear();
            OnCleared();
        }

        public bool Contains(T item)
        {
            TkDebug.AssertArgumentNull(item, "item", this);

            return fDictionary.ContainsKey(item.RegName);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo((Array)array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return fList.Count;
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
            TkDebug.AssertArgumentNull(item, "item", this);

            ListNode<T> node;
            if (fDictionary.TryGetValue(item.RegName, out node))
            {
                int index = fList.IndexOf(node);
                fDictionary.Remove(item.RegName);
                fList.Remove(node);
                OnRemoved(item, index);
                node = null;
                return true;
            }
            return false;
        }

        #endregion ICollection<T> 成员

        #region IEnumerable<T> 成员

        public IEnumerator<T> GetEnumerator()
        {
            return fList.GetEnumerator();
        }

        #endregion IEnumerable<T> 成员

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return fList.GetEnumerator();
        }

        #endregion IEnumerable 成员

        #region IList 成员

        int IList.Add(object value)
        {
            TkDebug.AssertArgumentNull(value, "value", this);

            int index = Count;
            Add((T)value);
            return index;
        }

        void IList.Clear()
        {
            Clear();
        }

        bool IList.Contains(object value)
        {
            TkDebug.AssertArgumentNull(value, "value", this);

            return Contains((T)value);
        }

        int IList.IndexOf(object value)
        {
            //TkDebug.AssertArgumentNull(value, "value", this);

            return IndexOf((T)value);
        }

        void IList.Insert(int index, object value)
        {
            TkDebug.AssertArgumentNull(value, "value", this);

            Insert(index, (T)value);
        }

        bool IList.IsFixedSize
        {
            get
            {
                return IsFixedSize;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return IsReadOnly;
            }
        }

        void IList.Remove(object value)
        {
            TkDebug.AssertArgumentNull(value, "value", this);

            Remove((T)value);
        }

        void IList.RemoveAt(int index)
        {
            RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (T)value;
            }
        }

        #endregion IList 成员

        #region ICollection 成员

        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo(array, index);
        }

        int ICollection.Count
        {
            get
            {
                return Count;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return SyncRoot;
            }
        }

        #endregion ICollection 成员

        protected static bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        protected static bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        protected object SyncRoot
        {
            get
            {
                if (fSyncRoot == null)
                    Interlocked.CompareExchange(ref fSyncRoot, new object(), null);
                return fSyncRoot;
            }
        }

        protected void CopyTo(Array array, int index)
        {
            TkDebug.AssertArgumentNull(array, "array", this);
            AssertIndex(index, array.Length);
            TkDebug.AssertArgument(array.Length - index >= Count, "arrayIndex", string.Format(
                ObjectUtil.SysCulture, "当前有{0}个元素，而数组的空间为{1}，空间不够",
                Count, array.Length - index), this);

            fList.CopyTo(array, index);
        }

        private void InternalAdd(T item)
        {
            if (!fDictionary.ContainsKey(item.RegName))
            {
                int index = fList.Count;
                ListNode<T> node = fList.AddLast(item);
                fDictionary.Add(item.RegName, node);
                OnAdded(item, index);
            }
        }

        [Conditional(ToolkitConst.DEBUG)]
        private void AssertIndex(int index, int count)
        {
            TkDebug.AssertArgument(index >= 0 && index < count, "index", string.Format(
                ObjectUtil.SysCulture, "index必须在0和{0}之间，现在值为{1}，已经超出范围",
                count, index), this);
        }

        public bool ConstainsKey(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            return fDictionary.ContainsKey(name);
        }

        public T this[string name]
        {
            get
            {
                TkDebug.AssertArgumentNullOrEmpty(name, "name", this);

                ListNode<T> node;
                if (fDictionary.TryGetValue(name, out node))
                    return node.Value;
                return default(T);
            }
        }

        public void AddOrReplace(T item)
        {
            TkDebug.AssertArgumentNull(item, "item", this);

            ListNode<T> node;
            if (fDictionary.TryGetValue(item.RegName, out node))
                node.Value = item;
            else
                Add(item);
        }

        protected virtual void OnAdded(T item, int index)
        {
        }

        protected virtual void OnRemoved(T item, int index)
        {
        }

        protected virtual void OnCleared()
        {
        }

        public void Sort(IComparer<T> comparer)
        {
            TkDebug.AssertArgumentNull(comparer, "comparer", this);

            var sortedList = fList.OrderBy(a => a, comparer).ToArray();
            fDictionary.Clear();
            fList.Clear();
            foreach (var item in sortedList)
            {
                ListNode<T> node = fList.AddLast(item);
                fDictionary.Add(item.RegName, node);
            }
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "{{Count={0}}}", Count);
        }
    }
}