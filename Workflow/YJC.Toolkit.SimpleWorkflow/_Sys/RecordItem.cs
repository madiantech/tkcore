using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class RecordItem
    {
        private readonly List<FieldItem> fFieldItems;

        public RecordItem()
        {
            fFieldItems = new List<FieldItem>();
        }

        internal RecordItem(string[] keys, string[] values)
            : this()
        {
            int lengthKeys = keys.Length;
            int lengthValues = values.Length;
            TkDebug.Assert(lengthKeys == lengthValues, "字段Key/Value必须成对", this);
            for (int i = 0; i < lengthKeys; i++)
            {
                AddFieldItem(keys[i], values[i]);
            }
        }

        internal RecordItem(string key, string value)
            : this()
        {
            AddFieldItem(key, value);
        }

        internal void AddFieldItem(string key, string value)
        {
            FieldItem fieldItem = new FieldItem(key, value);
            fFieldItems.Add(fieldItem);
        }

        [ObjectElement(LocalName = "Field", IsMultiple = true,
            ObjectType = typeof(FieldItem))]
        public List<FieldItem> FieldItems
        {
            get
            {
                return fFieldItems;
            }
        }
    }
}