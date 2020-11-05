using System.Collections;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Collections
{
    public abstract class BaseDictionaryList<TKey, TValue> : IEnumerable<TValue> where TValue : class
    {
        private readonly List<TValue> fList;
        private readonly Dictionary<TKey, TValue> fHashtable;

        /// <summary>
        /// Initializes a new instance of the DictionaryList class.
        /// </summary>
        protected BaseDictionaryList()
        {
            fList = new List<TValue>();
            fHashtable = new Dictionary<TKey, TValue>();
        }

        #region IEnumerable<T> 成员

        public IEnumerator<TValue> GetEnumerator()
        {
            return fList.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return fList.GetEnumerator();
        }

        #endregion

        protected void AddItem(TKey key, TValue instance)
        {
            TkDebug.AssertArgumentNull(key, "key", this);
            TkDebug.AssertArgumentNull(instance, "instance", this);

            if (!fHashtable.ContainsKey(key))
            {
                fHashtable.Add(key, instance);
                fList.Add(instance);
            }
        }

        protected bool ContainsKey(TKey key)
        {
            return fHashtable.ContainsKey(key);
        }

        public void Clear()
        {
            fList.Clear();
            fHashtable.Clear();
        }

        public int Count
        {
            get
            {
                return fList.Count;
            }
        }

        public TValue this[int index]
        {
            get
            {
                return fList[index];
            }
        }

        public TValue this[TKey regName]
        {
            get
            {
                TkDebug.AssertArgumentNull(regName, "regName", this);
                TkDebug.Assert(fHashtable.ContainsKey(regName), string.Format(
                    ObjectUtil.SysCulture, "集合中不含注册名为{0}的对象", regName), this);

                return fHashtable[regName];
            }
        }
    }
}
