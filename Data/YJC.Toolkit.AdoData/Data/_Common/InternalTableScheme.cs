using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class InternalTableScheme : ITableScheme
    {
        private RegNameList<InternalFieldConfigItem> fFieldInfos;

        public InternalTableScheme(string tableName, string keyFields, DataTable table)
        {
            string[] keyFieldArray = keyFields.Split(',');
            for (int i = 0; i < keyFieldArray.Length; i++)
                keyFieldArray[i] = keyFieldArray[i].Trim();
            Array.Sort(keyFieldArray);

            Initialize(tableName, table, keyFieldArray);
        }

        public InternalTableScheme(string tableName, DataTable table)
        {
            DataColumn[] keys = table.PrimaryKey;
            TkDebug.Assert(keys != null && keys.Length > 0, string.Format(ObjectUtil.SysCulture,
                "没有从表{0}中获取主键信息，请去数据库确认该表是否设置了主键", tableName), this);

            string[] keyFieldArray = Array.ConvertAll(keys, column => column.ColumnName);
            Array.Sort(keyFieldArray);

            Initialize(tableName, table, keyFieldArray);
        }

        #region ITableScheme 成员

        public string TableName { get; private set; }

        public IEnumerable<IFieldInfo> Fields
        {
            get
            {
                return fFieldInfos;
            }
        }

        public IEnumerable<IFieldInfo> AllFields
        {
            get
            {
                return fFieldInfos;
            }
        }

        public IFieldInfo this[string nickName]
        {
            get
            {
                return fFieldInfos[nickName];
            }
        }

        #endregion ITableScheme 成员

        private void Initialize(string tableName, DataTable table, string[] keyFieldArray)
        {
            TableName = tableName;
            DataColumnCollection columns = table.Columns;
            fFieldInfos = new RegNameList<InternalFieldConfigItem>();
            foreach (DataColumn column in columns)
                fFieldInfos.Add(new InternalFieldConfigItem(column, keyFieldArray));
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "表{0}的TableScheme", TableName);
        }
    }
}