using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SysFunction
{
    internal class CustomField : IFieldInfo, IFieldInfoEx, IRegName, IReadObjectCallBack
    {
        private IFieldControl fControl;

        public CustomField()
        {
            Layout = SimpleFieldLayout.CreateDefault();
        }

        #region IFieldInfo 成员

        [SimpleAttribute]
        public TkDataType DataType { get; private set; }

        [SimpleAttribute]
        public string DisplayName { get; private set; }

        [SimpleAttribute]
        public string FieldName { get; private set; }

        public bool IsAutoInc
        {
            get
            {
                return false;
            }
        }

        public bool IsKey
        {
            get
            {
                return Character == 1 || Character == 2;
            }
        }

        [SimpleAttribute]
        public string NickName { get; private set; }

        #endregion IFieldInfo 成员

        #region IFieldInfoEx 成员

        IFieldControl IFieldInfoEx.Control
        {
            get
            {
                if (fControl == null)
                {
                    PageStyle style;
                    ControlType ctrl = Control;
                    switch (Character)
                    {
                        case 8:
                            style = PageStyle.All;
                            ctrl = ControlType.Hidden;
                            break;

                        case 9:
                            style = PageStyle.None;
                            break;

                        default:
                            style = PageStyle.All;
                            break;
                    }
                    fControl = new SimpleFieldControl(ctrl, FieldOrder, style);
                }
                return fControl;
            }
        }

        [ObjectElement(LocalName = "Extionsion1", ObjectType = typeof(SimpleFieldDecoder))]
        public IFieldDecoder Decoder { get; private set; }

        public string Expression
        {
            get
            {
                return null;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return Character == 5 || Character == 3 || Character == 4;
            }
        }

        public FieldKind Kind
        {
            get
            {
                return FieldKind.Data;
            }
        }

        public IFieldLayout Layout { get; private set; }

        [SimpleAttribute]
        public int Length { get; private set; }

        public int Precision
        {
            get
            {
                return 0;
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
            var ctrl = this.Convert<IFieldInfoEx>().Control.Convert<SimpleFieldControl>();
            if ((ctrl.DefaultShow & PageStyle.List) != PageStyle.List)
                return false;

            if (isInTable)
                return ctrl.SrcControl != ControlType.Hidden;
            return true;
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

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (string.IsNullOrEmpty(NickName))
                NickName = StringUtil.GetNickName(FieldName);
        }

        #endregion IReadObjectCallBack 成员

        [SimpleAttribute]
        public int RealLength { get; private set; }

        [SimpleAttribute]
        public ControlType Control { get; private set; }

        [SimpleAttribute]
        public int Character { get; private set; }

        [SimpleAttribute]
        public int FieldOrder { get; private set; }

        [SimpleAttribute]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Display { get; private set; }

        [SimpleAttribute]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Query { get; private set; }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "[{0}, {1}]", NickName, DataType);
        }
    }
}