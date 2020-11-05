using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public class FieldItem : IFieldInfo, IRegName, IReadObjectCallBack
    {
        private string fNickName;
        private string fFieldName;

        protected FieldItem()
        {
        }

        public FieldItem(IFieldInfo fieldInfo)
        {
            TkDebug.AssertArgumentNull(fieldInfo, "fieldInfo", null);

            fFieldName = fieldInfo.FieldName;
            fNickName = fieldInfo.NickName;
            DataType = fieldInfo.DataType;
        }

        public FieldItem(string fieldName)
            : this(fieldName, TkDataType.String)
        {
        }

        public FieldItem(string fieldName, TkDataType dataType)
            : this(fieldName, StringUtil.GetNickName(fieldName), dataType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the FieldItem class.
        /// </summary>
        public FieldItem(string fieldName, string nickName, TkDataType dataType)
        {
            TkDebug.AssertArgumentNullOrEmpty(fieldName, "fieldName", null);
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", null);

            fFieldName = fieldName;
            fNickName = nickName;
            DataType = dataType;
        }


        #region IRegName 成员

        public string RegName
        {
            get
            {
                return NickName;
            }
        }

        #endregion

        #region IFieldInfo 成员

        [SimpleAttribute]
        public virtual string NickName
        {
            get
            {
                return fNickName;
            }
            protected set
            {
                fNickName = value;
            }
        }

        string IFieldInfo.FieldName
        {
            get
            {
                return FieldName;
            }
        }

        string IFieldInfo.DisplayName
        {
            get
            {
                return DisplayName;
            }
        }

        TkDataType IFieldInfo.DataType
        {
            get
            {
                return DataType;
            }
        }

        bool IFieldInfo.IsKey
        {
            get
            {
                return IsKey;
            }
        }

        bool IFieldInfo.IsAutoInc
        {
            get
            {
                return IsAutoInc;
            }
        }

        #endregion

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (string.IsNullOrEmpty(NickName))
                NickName = StringUtil.GetNickName(FieldName);
        }

        #endregion

        [TextContent]
        public virtual string FieldName
        {
            get
            {
                return fFieldName;
            }
            protected set
            {
                fFieldName = value;
            }
        }

        [SimpleAttribute(DefaultValue = TkDataType.String)]
        public TkDataType DataType { get; protected set; }

        protected internal string DisplayName
        {
            get
            {
                return FieldName;
            }
        }

        protected virtual internal bool IsKey
        {
            get
            {
                return false;
            }
        }

        protected static internal bool IsAutoInc
        {
            get
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            FieldItem item = obj as FieldItem;
            if (FieldItem.Equals(item, null))
                return false;
            return NickName == item.NickName;
        }

        public override int GetHashCode()
        {
            return NickName.GetHashCode();
        }

        public static bool operator ==(FieldItem item, string nickName)
        {
            if (FieldItem.Equals(item, null) && nickName == null)
                return true;
            if (FieldItem.Equals(item, null) || nickName == null)
                return false;
            return item.NickName == nickName;
        }

        public static bool operator !=(FieldItem item, string nickName)
        {
            if (FieldItem.Equals(item, null) && nickName == null)
                return false;
            if (FieldItem.Equals(item, null) || nickName == null)
                return true;
            return item.NickName != nickName;
        }

        public static implicit operator FieldItem(string fieldName)
        {
            return ToFieldItem(fieldName);
        }

        internal static FieldItem ToFieldItem(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return null;
            return new FieldItem(fieldName);
        }

        //internal static void SetFieldItemValue(FieldItem field, FieldItem value,
        //    Dictionary<string, IFieldInfo> fields, string tableName)
        //{
        //    if (field != null)
        //        fields.Remove(field.FieldName);
        //    if (value != null)
        //    {
        //        TkDebug.Assert(!fields.ContainsKey(value.FieldName),
        //            string.Format(ObjectUtil.SysCulture,
        //            "表{0}中已经包含字段{1}了，不能继续增加同名的字段",
        //            tableName, value.FieldName), null);
        //        fields.Add(value.FieldName, value);
        //    }
        //}

        public override string ToString()
        {
            if (FieldName == null)
                return base.ToString();
            return string.Format(ObjectUtil.SysCulture, "[{0}, {1}]", FieldName, DataType);
        }
    }
}
