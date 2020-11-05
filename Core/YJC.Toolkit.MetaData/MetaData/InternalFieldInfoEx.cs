namespace YJC.Toolkit.MetaData
{
    internal class InternalFieldInfoEx : IFieldInfoEx
    {
        private FieldLayoutAttribute fLayout;
        private FieldControlAttribute fControl;
        private readonly IFieldInfo fFieldInfo;

        public InternalFieldInfoEx(IFieldInfo fieldInfo)
        {
            fFieldInfo = fieldInfo;
        }

        #region IFieldInfoEx 成员

        public int Length
        {
            get
            {
                return 0;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return true;
            }
        }

        public int Precision
        {
            get
            {
                return 0;
            }
        }

        public FieldKind Kind
        {
            get
            {
                return FieldKind.Data;
            }
        }

        public string Expression
        {
            get
            {
                return string.Empty;
            }
        }

        public IFieldLayout Layout
        {
            get
            {
                if (fLayout == null)
                    fLayout = new FieldLayoutAttribute();
                return fLayout;
            }
        }

        public IFieldControl Control
        {
            get
            {
                if (fControl == null)
                    fControl = new FieldControlAttribute(ControlType.Text);
                return fControl;
            }
        }

        public IFieldDecoder Decoder
        {
            get
            {
                return null;
            }
        }

        public IFieldUpload Upload
        {
            get
            {
                return null;
            }
        }

        public bool IsShowInList(IPageStyle style, bool isInTable)
        {
            return true;
        }

        #endregion IFieldInfoEx 成员

        #region IFieldInfo 成员

        public string FieldName
        {
            get
            {
                return fFieldInfo.FieldName;
            }
        }

        public string DisplayName
        {
            get
            {
                return fFieldInfo.NickName;
            }
        }

        public string NickName
        {
            get
            {
                return fFieldInfo.NickName;
            }
        }

        public TkDataType DataType
        {
            get
            {
                return fFieldInfo.DataType;
            }
        }

        public bool IsKey
        {
            get
            {
                return fFieldInfo.IsKey;
            }
        }

        public bool IsAutoInc
        {
            get
            {
                return fFieldInfo.IsAutoInc;
            }
        }

        #endregion IFieldInfo 成员
    }
}