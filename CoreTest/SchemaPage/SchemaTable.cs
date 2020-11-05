using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite
{
    public class SchemaTable : ITableSchemeEx
    {
        private readonly RegNameList<SchemaField> fFields;
        private readonly string fRootName;

        public SchemaTable(string rootName)
        {
            fFields = new RegNameList<SchemaField>();
            fRootName = rootName;
        }

        public IFieldInfo this[string nickName] { get => fFields[nickName]; }

        [SimpleAttribute]
        public string TableName { get => fRootName; }

        [SimpleAttribute]
        public string TableDesc { get => fRootName; }

        public IFieldInfoEx NameField { get => null; }

        public int CurrentOrder { get => fFields.Count * 10; }

        [ObjectElement(IsMultiple = true, CollectionType = typeof(RegNameList<SchemaField>), ObjectType = typeof(SchemaField), LocalName = "Field")]
        public IEnumerable<IFieldInfoEx> Fields { get => fFields; }

        public IEnumerable<IFieldInfoEx> AllFields { get => fFields; }

        internal int AddField(SchemaField field)
        {
            fFields.Add(field);
            int order = fFields.Count * 10;
            field.SetOrder(order);
            return order;
        }
    }
}