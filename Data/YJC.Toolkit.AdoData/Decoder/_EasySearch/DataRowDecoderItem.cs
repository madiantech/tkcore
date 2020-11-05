using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public class DataRowDecoderItem : IDecoderItem
    {
        public DataRowDecoderItem(string value, string name)
        {
            Value = value;
            Name = name;
            DisplayName = Name;
        }

        internal DataRowDecoderItem(IDecoderItem item)
        {
            Value = item.Value;
            Name = item.Name;
            DisplayName = item.DisplayName;
        }

        internal DataRowDecoderItem(DataColumnCollection columns, DataRow row,
            string keyNickName, string nameNickName)
        {
            RowDatas = new Dictionary<string, string>();
            Value = row[keyNickName].ToString();
            Name = row[nameNickName].ToString();
            DisplayName = Name;

            foreach (DataColumn column in columns)
            {
                object value = row[column];
                if (value != DBNull.Value)
                    RowDatas[column.ColumnName] = value.ToString();
            }
        }

        internal DataRowDecoderItem(DataColumnCollection columns, DataRow row,
            string keyNickName, DecoderNameExpression nameExpression,
            DecoderNameExpression displayExression)
        {
            RowDatas = new Dictionary<string, string>();
            Value = row[keyNickName].ToString();
            Name = nameExpression.Execute(row);
            if (displayExression == null)
                DisplayName = Name;
            else
                DisplayName = displayExression.Execute(row);

            foreach (DataColumn column in columns)
            {
                object value = row[column];
                if (value != DBNull.Value)
                    RowDatas[column.ColumnName] = value.ToString();
            }
        }

        #region IDecoderItem 成员

        [SimpleAttribute]
        public string Value { get; private set; }

        [SimpleAttribute]
        public string Name { get; private set; }

        [SimpleAttribute]
        public string DisplayName { get; private set; }

        public string this[string name]
        {
            get
            {
                if (RowDatas == null)
                    return null;
                return ObjectUtil.TryGetValue(RowDatas, name);
            }
        }

        #endregion

        [Dictionary(LocalName = "Data")]
        internal Dictionary<string, string> RowDatas { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
