using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class ResourceTableScheme : ToolkitConfig, ITableScheme, IDisplayObject
    {
        #region IDisplayObject 成员

        public bool SupportDisplay { get; private set; }

        public IFieldInfo Id { get; private set; }

        public IFieldInfo Name { get; private set; }

        #endregion IDisplayObject 成员

        #region ITableScheme 成员

        public string TableName
        {
            get
            {
                return Table.TableName;
            }
        }

        public IEnumerable<IFieldInfo> Fields
        {
            get
            {
                return Table.FieldList;
            }
        }

        public IEnumerable<IFieldInfo> AllFields
        {
            get
            {
                return Table.FieldList;
            }
        }

        #endregion ITableScheme 成员

        #region IFieldInfoIndexer 成员

        public IFieldInfo this[string nickName]
        {
            get
            {
                return Table.FieldList[nickName];
            }
        }

        #endregion IFieldInfoIndexer 成员

        [ObjectElement(NamespaceType.Toolkit)]
        internal ResourceTableConfig Table { get; private set; }

        protected override void OnReadObject()
        {
            base.OnReadObject();

            SupportDisplay = false;
            var keyFields = (from item in Table.FieldList
                             where item.IsKey
                             select item).ToArray();
            if (keyFields.Length == 1)
            {
                Id = keyFields[0];
                if (string.IsNullOrEmpty(Table.NameField))
                {
                    Name = (from item in Table.FieldList
                            where item.NickName.EndsWith("Name", StringComparison.Ordinal)
                            select item).FirstOrDefault();
                    if (Name != null)
                        SupportDisplay = true;
                }
                else
                {
                    Name = Table.FieldList[Table.NameField];
                    if (Name != null)
                        SupportDisplay = true;
                }
            }
        }
    }
}