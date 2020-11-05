using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal sealed class InternalFieldConfigItem : IFieldInfo, IRegName
    {
        private readonly static IComparer<string> COMPARER = StringComparer.Create(ObjectUtil.SysCulture, true);

        public InternalFieldConfigItem()
        {
        }

        public InternalFieldConfigItem(DataColumn column, string[] sortedKeyFields)
        {
            FieldName = DisplayName = column.ColumnName;
            NickName = StringUtil.GetNickName(FieldName);
            DataType = MetaDataUtil.ConvertTypeToDataType(column.DataType);
            IsAutoInc = column.AutoIncrement;
            if (sortedKeyFields.Length == 1)
                IsKey = string.Compare(FieldName, sortedKeyFields[0], true, ObjectUtil.SysCulture) == 0;
            else
                IsKey = Array.BinarySearch(sortedKeyFields, FieldName, COMPARER) >= 0;
        }

        internal InternalFieldConfigItem(IFieldInfo item, bool isKey)
        {
            FieldName = DisplayName = item.FieldName;
            NickName = item.NickName;
            if (string.IsNullOrEmpty(NickName))
                NickName = StringUtil.GetNickName(FieldName);
            IsKey = isKey;
            DataType = item.DataType;
            IsAutoInc = item.IsAutoInc;
        }

        #region IFieldInfo

        [SimpleAttribute]
        public bool IsKey { get; private set; }

        [SimpleAttribute]
        public TkDataType DataType { get; private set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public string FieldName { get; private set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public string DisplayName { get; private set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public string NickName { get; private set; }

        [SimpleAttribute]
        public bool IsAutoInc { get; private set; }

        #endregion

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return NickName;
            }
        }

        #endregion

        public override string ToString()
        {
            return string.IsNullOrEmpty(FieldName) ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "{{{0}, {1}}}", FieldName, DataType);
        }
    }
}
