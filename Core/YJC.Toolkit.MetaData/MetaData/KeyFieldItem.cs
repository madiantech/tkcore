namespace YJC.Toolkit.MetaData
{
    public class KeyFieldItem : FieldItem
    {
        protected KeyFieldItem()
        {
        }

        public KeyFieldItem(string fieldName)
            : base(fieldName)
        {
        }

        public KeyFieldItem(string fieldName, TkDataType dataType)
            : base(fieldName, dataType)
        {
        }

        public KeyFieldItem(string fieldName, string nickName, TkDataType dataType)
            : base(fieldName, nickName, dataType)
        {
        }

        public KeyFieldItem(IFieldInfo fieldInfo)
            : base(fieldInfo)
        {
        }

        protected internal override bool IsKey
        {
            get
            {
                return true;
            }
        }
    }
}
