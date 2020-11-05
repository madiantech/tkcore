using YJC.Toolkit.Decoder;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    internal class CodeTableFieldInfoEx : IFieldInfoEx, IFieldInfo, IRegName
    {
        private FieldLayoutAttribute fLayout;
        private FieldControlAttribute fControl;

        private CodeTableFieldInfoEx(string fieldName, string displayName, TkDataType dataType, bool isKey, bool isEmpty)
        {
            FieldName = fieldName;
            DisplayName = displayName;
            DataType = dataType;
            IsKey = isKey;
            IsEmpty = isEmpty;
            NickName = StringUtil.GetNickName(fieldName);
            if (dataType == TkDataType.String)
                Length = 50;
        }

        #region IFieldInfo 成员

        public string FieldName { get; private set; }

        public string DisplayName { get; private set; }

        public string NickName { get; private set; }

        public TkDataType DataType { get; private set; }

        public bool IsKey { get; private set; }

        public bool IsAutoInc
        {
            get
            {
                return false;
            }
        }

        #endregion IFieldInfo 成员

        #region IFieldInfoEx 成员

        public int Length { get; private set; }

        public bool IsEmpty { get; private set; }

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
                VerifyControl();
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
            return SchemeUtil.IsShowInList(Control, null, Kind, style, isInTable);
        }

        #endregion IFieldInfoEx 成员

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return NickName;
            }
        }

        #endregion IRegName 成员

        private void VerifyControl()
        {
            var type = IsKey || FieldName == DecoderConst.DEL_FIELD_NAME ? ControlType.Hidden : ControlType.Text;
            if (fControl == null)
                fControl = new FieldControlAttribute(type);
        }

        public CodeTableFieldInfoEx SetDefaultShow(PageStyle style)
        {
            VerifyControl();
            fControl.DefaultShow = style;
            return this;
        }

        public CodeTableFieldInfoEx SetControl(ControlType ctrl)
        {
            VerifyControl();
            if (fControl.Control != ctrl)
                fControl = new FieldControlAttribute(ctrl);
            return this;
        }

        public static CodeTableFieldInfoEx CreateValueField()
        {
            var result = new CodeTableFieldInfoEx(DecoderConst.CODE_FIELD_NAME, "值",
                TkDataType.String, true, false);
            return result.SetControl(ControlType.Hidden);
        }

        public static CodeTableFieldInfoEx CreateNameField()
        {
            return new CodeTableFieldInfoEx(DecoderConst.NAME_FIELD_NAME, "名称",
                TkDataType.String, false, false);
        }

        public static CodeTableFieldInfoEx CreatePyField(string caption)
        {
            string displayName = string.IsNullOrEmpty(caption) ? "拼音" : caption;
            var result = new CodeTableFieldInfoEx(DecoderConst.PY_FIELD_NAME, displayName,
                TkDataType.String, false, true);
            return result.SetDefaultShow(PageStyle.List | PageStyle.Update);
        }

        public static CodeTableFieldInfoEx CreateSortField()
        {
            var result = new CodeTableFieldInfoEx(DecoderConst.SORT_FIELD_NAME, "排序",
                TkDataType.Int, false, true);
            return result.SetDefaultShow(PageStyle.None);
        }

        public static CodeTableFieldInfoEx CreateActiveField()
        {
            var result = new CodeTableFieldInfoEx(DecoderConst.DEL_FIELD_NAME,
                "删除标志", TkDataType.Short, false, true);
            return result.SetDefaultShow(PageStyle.List);
        }
    }
}