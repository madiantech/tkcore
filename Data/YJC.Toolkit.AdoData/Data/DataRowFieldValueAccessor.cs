using System;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class DataRowFieldValueAccessor : DataRowFieldValueProvider, IFieldValueAccessor
    {
        public DataRowFieldValueAccessor(DataRow row, DataSet dataSet)
            : base(row, dataSet)
        {
        }

        #region IFieldValueAccessor 成员

        public object GetOriginValue(string nickName)
        {
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", this);

            if (IsEmpty)
                return DBNull.Value;
            else
                return Row[nickName, DataRowVersion.Original];
        }

        public void SetValue(string nickName, object value)
        {
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", this);

            if (Row != null)
                Row[nickName] = value;
        }

        #endregion
    }
}
