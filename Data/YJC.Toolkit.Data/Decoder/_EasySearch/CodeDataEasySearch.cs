using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public class CodeDataEasySearch<T> : EasySearch where T : IDecoderItem, IRegName
    {
        private IDataSearch fSearchMethod;
        private readonly RegNameList<EasySearchDataItem<T>> fList;

        public CodeDataEasySearch()
        {
            fList = new RegNameList<EasySearchDataItem<T>>();
            fSearchMethod = DefaultDataSearch.SearchMethod;
        }

        public CodeDataEasySearch(IEnumerable<T> list)
            : this()
        {
            if (list != null)
                foreach (var item in list)
                    Add(item);
        }


        public IDataSearch SearchMethod
        {
            get
            {
                return fSearchMethod;
            }
            set
            {
                if (fSearchMethod != value)
                {
                    if (value == null)
                        fSearchMethod = DefaultDataSearch.SearchMethod;
                    else
                        fSearchMethod = value;
                }
            }
        }

        public void Add(T item)
        {
            if (item == null)
                return;

            fList.Add(new EasySearchDataItem<T>(item));
        }

        public override IDecoderItem Decode(string code, params object[] args)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            EasySearchDataItem<T> result = fList[code];
            if (result != null)
                return result.Item;
            return null;
        }

        private static string GetDataValue(SearchField field, EasySearchDataItem<T> item)
        {
            switch (field)
            {
                case SearchField.Value:
                    return item.Item.Value;
                case SearchField.Name:
                    return item.Item.Name;
                case SearchField.Pinyin:
                    return item.Pinyin;
            }
            return null;
        }

        private static bool CompareRefFields(object receiver, List<EasySearchRefField> refFields)
        {
            if (refFields == null || refFields.Count == 0)
                return true;

            try
            {
                foreach (var field in refFields)
                {
                    var objValue = receiver.MemberValue(field.NickName);
                    if (objValue.ConvertToString() != field.Value)
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override IEnumerable<IDecoderItem> Search(SearchField field, string text,
            List<EasySearchRefField> refFields)
        {
            var result = from item in fList
                         where fSearchMethod.Search(this, field, GetDataValue(field, item), text)
                         && CompareRefFields(item, refFields)
                         select item.Item;
            int count = 0;
            List<IDecoderItem> list = new List<IDecoderItem>();
            foreach (var item in result)
            {
                if (++count < TopCount)
                    list.Add(new CodeItem(item));
                else
                    break;
            }

            return list;
        }

        public override IDecoderItem[] SearchByName(string name, params object[] args)
        {
            var result = from item in fList
                         where item.Item.Name == name
                         select item.Item;
            List<IDecoderItem> list = new List<IDecoderItem>(2);
            int count = 0;
            foreach (var item in result)
            {
                list.Add(item);
                if (++count >= 2)
                    break;
            }
            return list.ToArray();
        }
    }
}
