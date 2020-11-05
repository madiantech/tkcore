using System.Collections.Generic;
using YJC.Toolkit.Data.Constraint;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    internal sealed class FieldConfigItem : IRegName, IFieldInfoEx, IReadObjectCallBack, ITk5FieldInfo
    {
        private readonly static IFieldLayout DEFAULT_LAYOUT = SimpleFieldLayout.CreateDefault();

        private SimpleFieldDecoder fDecoder;

        #region IRegName 成员

        string IRegName.RegName
        {
            get
            {
                return NickName;
            }
        }

        #endregion IRegName 成员

        #region IFieldInfo 成员

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
                return ToString(DisplayName, string.Empty);
            }
        }

        string IFieldInfo.NickName
        {
            get
            {
                return NickName;
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

        #endregion IFieldInfo 成员

        #region IFieldInfoEx 成员

        int IFieldInfoEx.Length
        {
            get
            {
                return Length;
            }
        }

        bool IFieldInfoEx.IsEmpty
        {
            get
            {
                return IsEmpty;
            }
        }

        int IFieldInfoEx.Precision
        {
            get
            {
                return Precision;
            }
        }

        FieldKind IFieldInfoEx.Kind
        {
            get
            {
                return Kind;
            }
        }

        string IFieldInfoEx.Expression
        {
            get
            {
                return Extension != null ? Extension.Expression : null;
            }
        }

        IFieldLayout IFieldInfoEx.Layout
        {
            get
            {
                return Layout ?? DEFAULT_LAYOUT;
            }
        }

        IFieldControl IFieldInfoEx.Control
        {
            get
            {
                return Control;
            }
        }

        IFieldDecoder IFieldInfoEx.Decoder
        {
            get
            {
                return fDecoder;
            }
        }

        IFieldUpload IFieldInfoEx.Upload
        {
            get
            {
                return Upload;
            }
        }

        public bool IsShowInList(IPageStyle style, bool isInTable)
        {
            return SchemeUtil.IsShowInList(Control, ListDetail, Kind, style, isInTable);
        }

        #endregion IFieldInfoEx 成员

        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (string.IsNullOrEmpty(NickName))
                NickName = StringUtil.GetNickName(FieldName);
            fDecoder = new SimpleFieldDecoder(this);
            if (Upload != null)
                Upload.FileNameField = NickName;
        }

        #endregion IReadObjectCallBack 成员

        [SimpleAttribute(Required = true)]
        public TkDataType DataType { get; private set; }

        [SimpleAttribute]
        public bool IsKey { get; private set; }

        [SimpleAttribute]
        public bool IsAutoInc { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool IsEmpty { get; private set; }

        [SimpleAttribute]
        public int Precision { get; private set; }

        [SimpleAttribute(DefaultValue = FieldKind.Data)]
        public FieldKind Kind { get; private set; }

        [SimpleElement(NamespaceType.Toolkit, Required = true)]
        public string FieldName { get; private set; }

        [SimpleElement(NamespaceType.Toolkit, AutoTrim = true)]
        public string NickName { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public MultiLanguageText DisplayName { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool PlaceHolder { get; set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public HintData Hint { get; private set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public int Length { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public DecoderConfigItem CodeTable { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public EasySearchConfigItem EasySearch { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public SimpleFieldLayout Layout { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public ControlConfigItem Control { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(ConstraintConfigFactory.REG_NAME, IsMultiple = true)]
        public List<IConfigCreator<BaseConstraint>> Constraints { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public Tk5ListDetailConfig ListDetail { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public Tk5EditConfig Edit { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public Tk5ExtensionConfig Extension { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public Tk5UploadConfig Upload { get; private set; }

        internal static string ToString(MultiLanguageText lang, string defaultValue)
        {
            return lang == null ? defaultValue : lang.ToString();
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "[{0}, {1}]", NickName, DataType);
        }
    }
}