using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public class UnionScheme : ITableSchemeEx
    {
        private readonly RegNameList<UnionFieldInfoEx> fFields;

        public UnionScheme(string tableName, string tableDesc, string nameField, IEnumerable<ITableSchemeEx> schemes)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);
            TkDebug.AssertArgumentNullOrEmpty(tableDesc, "tableDesc", null);
            TkDebug.AssertArgumentNull(schemes, "schemes", null);

            TableName = tableName;
            TableDesc = tableDesc;
            fFields = new RegNameList<UnionFieldInfoEx>();
            foreach (var scheme in schemes)
            {
                foreach (var field in scheme.Fields)
                {
                    if (!fFields.ConstainsKey(field.NickName))
                    {
                        if (field is UnionFieldInfoEx)
                            fFields.Add((UnionFieldInfoEx)field);
                        else
                            fFields.Add(new UnionFieldInfoEx(field, null));
                    }
                }
            }
            if (string.IsNullOrEmpty(nameField))
                NameField = schemes.FirstOrDefault()?.NameField;
            else
                NameField = fFields[nameField];
        }

        public string TableName { get; private set; }

        public string TableDesc { get; private set; }

        public IEnumerable<IFieldInfoEx> Fields
        {
            get
            {
                return fFields;
            }
        }

        public IEnumerable<IFieldInfoEx> AllFields
        {
            get
            {
                return fFields;
            }
        }

        public IFieldInfoEx NameField { get; }

        public IFieldInfo this[string nickName]
        {
            get
            {
                return fFields[nickName];
            }
        }
    }
}