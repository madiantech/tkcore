using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal sealed class DbParameter : IDbParameter, IReadObjectCallBack
    {
        public DbParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DbParameter class.
        /// </summary>
        public DbParameter(string fieldName, TkDataType dataType, object fieldValue)
        {
            FieldName = fieldName;
            DataType = dataType;
            FieldValue = fieldValue;
            if (fieldValue != null)
                Value = ObjectUtil.ToString(fieldValue, ObjectUtil.WriteSettings);
        }

        [SimpleAttribute]
        public string FieldName { get; private set; }

        [SimpleAttribute]
        public TkDataType DataType { get; private set; }

        public object FieldValue { get; private set; }

        [SimpleAttribute]
        internal string Value { get; private set; }

        public override string ToString()
        {
            return base.ToString();
        }

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (string.IsNullOrEmpty(Value))
                return;
            Type type = MetaDataUtil.ConvertDataTypeToType(DataType);
            if (type != typeof(byte[]))
            {
                FieldValue = ObjectUtil.GetValue(this, type, Value, null, ObjectUtil.ReadSettings);
            }
        }

        #endregion
    }
}
