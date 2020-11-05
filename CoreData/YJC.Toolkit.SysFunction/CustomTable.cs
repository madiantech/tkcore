using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SysFunction
{
    internal class CustomTable : ITableScheme, ITableSchemeEx, IDisplayObject
    {
        private readonly RegNameList<CustomField> fFields;

        public CustomTable(DataRow tableRow, DataTable fieldTable)
        {
            fFields = new RegNameList<CustomField>();
            TableData = tableRow.ReadFromDataRow<CustomTableData>();

            foreach (DataRow row in fieldTable.Rows)
            {
                CustomField field = row.ReadFromDataRow<CustomField>();
                fFields.Add(field);
            }
            string nameField = TableData.NameField;
            if (!string.IsNullOrEmpty(nameField))
            {
                var field = fFields[nameField];
                if (field != null)
                {
                    SupportDisplay = true;
                    NameField = field;
                    Name = field;
                    var keyField = (from item in fFields
                                    where item.IsKey
                                    select item).FirstOrDefault();
                    Id = keyField;
                }
            }
        }

        #region ITableSchemeEx 成员

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

        public string TableDesc
        {
            get
            {
                return TableData.DisplayName;
            }
        }

        public string TableName
        {
            get
            {
                return TableData.TableName;
            }
        }

        #endregion ITableSchemeEx 成员

        #region IFieldInfoIndexer 成员

        public IFieldInfo this[string nickName]
        {
            get
            {
                TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", this);

                return fFields[nickName];
            }
        }

        #endregion IFieldInfoIndexer 成员

        #region IDisplayObject 成员

        public IFieldInfo Id { get; private set; }

        public IFieldInfo Name { get; private set; }

        public bool SupportDisplay { get; private set; }

        #endregion IDisplayObject 成员

        public CustomTableData TableData { get; private set; }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "[Count={0}]", fFields.Count);
        }

        #region ITableScheme 成员

        IEnumerable<IFieldInfo> ITableScheme.Fields
        {
            get
            {
                return fFields;
            }
        }

        IEnumerable<IFieldInfo> ITableScheme.AllFields
        {
            get
            {
                return fFields;
            }
        }

        public IFieldInfoEx NameField { get; }

        #endregion ITableScheme 成员
    }
}