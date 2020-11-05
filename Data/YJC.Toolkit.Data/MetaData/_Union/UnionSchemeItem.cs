using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    internal class UnionSchemeItem
    {
        private class InternalScheme : ITableSchemeEx
        {
            private readonly RegNameList<UnionFieldInfoEx> fFields;
            private readonly ITableSchemeEx fScheme;

            public InternalScheme(ITableSchemeEx scheme, RegNameList<UnionFieldInfoEx> fields)
            {
                fScheme = scheme;
                fFields = fields;
            }

            public string TableName
            {
                get
                {
                    return fScheme.TableName;
                }
            }

            public string TableDesc
            {
                get
                {
                    return fScheme.TableDesc;
                }
            }

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

            public IFieldInfoEx NameField { get => fScheme.NameField; }

            public IFieldInfo this[string nickName]
            {
                get
                {
                    return fFields[nickName];
                }
            }
        }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(TableSchemeExConfigFactory.REG_NAME, Required = true)]
        public IConfigCreator<ITableSchemeEx> Scheme { get; private set; }

        [SimpleElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "ExceptField")]
        public HashList<string> ExceptFields { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "NickNameAlias")]
        public RegNameList<NickNameAlias> NickNameAliases { get; private set; }

        public ITableSchemeEx CreateScheme()
        {
            ITableSchemeEx scheme = Scheme.CreateObject();
            RegNameList<UnionFieldInfoEx> list = new RegNameList<UnionFieldInfoEx>();
            foreach (var field in scheme.Fields)
            {
                if (ExceptFields != null)
                {
                    if (ExceptFields.Contains(field.NickName))
                        continue;
                }
                UnionFieldInfoEx newField;
                if (NickNameAliases != null && NickNameAliases.ConstainsKey(field.NickName))
                {
                    newField = new UnionFieldInfoEx(field, NickNameAliases[field.NickName].NewNickName);
                }
                else
                {
                    newField = new UnionFieldInfoEx(field, null);
                }
                list.Add(newField);
            }

            InternalScheme result = new InternalScheme(scheme, list);
            return result;
        }
    }
}