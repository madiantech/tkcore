using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class ListSearchCollection
    {
        private readonly Dictionary<string, BaseListSearch> fList;

        internal ListSearchCollection()
        {
            fList = new Dictionary<string, BaseListSearch>();
        }

        public BaseListSearch this[string nickName]
        {
            get
            {
                TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", this);
                return ObjectUtil.TryGetValue(fList, nickName);
            }
        }

        internal void InternalAdd(IFieldInfo field, BaseListSearch listSearch)
        {
            listSearch.FieldName = field;
            fList.Add(field.NickName, listSearch);
        }

        public void Add(IFieldInfo field, BaseListSearch listSearch)
        {
            TkDebug.AssertArgumentNull(field, "field", this);
            TkDebug.AssertArgumentNull(listSearch, "listSearch", this);

            listSearch.FieldName = field;
            fList[field.NickName] = listSearch;
            //fList.Add(field.FieldName, listSearch);
        }

        public void AddSpan(IFieldInfo field, BaseListSearch smallSearch, BaseListSearch largeSearch)
        {
            TkDebug.AssertArgumentNull(field, "field", this);
            TkDebug.AssertArgumentNull(smallSearch, "smallSearch", this);
            TkDebug.AssertArgumentNull(largeSearch, "largeSearch", this);

            smallSearch.FieldName = field;
            largeSearch.FieldName = field;
            fList.Add(field.NickName, smallSearch);
            fList.Add(field.NickName + "END", largeSearch);
        }

        public bool Contains(string nickName)
        {
            return fList.ContainsKey(nickName);
        }

        public void Clear()
        {
            fList.Clear();
        }
    }
}
