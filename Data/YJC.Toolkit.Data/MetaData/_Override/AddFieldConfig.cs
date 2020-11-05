using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [ObjectContext]
    internal class AddFieldConfig : BaseFieldConfig, IReadObjectCallBack
    {
        #region IReadObjectCallBack 成员

        public void OnReadObject()
        {
            if (string.IsNullOrEmpty(NickName))
                NickName = StringUtil.GetNickName(FieldName);
        }

        #endregion

        [SimpleAttribute(Required = true)]
        public string FieldName { get; private set; }

        [SimpleAttribute(Required = true)]
        public TkDataType DataType { get; private set; }

        [SimpleAttribute(Required = true)]
        public ControlType Control { get; private set; }

        [SimpleAttribute(Required = true)]
        public int Order { get; private set; }

        [SimpleAttribute(Required = true)]
        public FieldKind Kind { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool IsEmpty { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool PlaceHolder { get; private set; }

        [SimpleAttribute]
        public int Length { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public MultiLanguageText DisplayName { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public HintData Hint { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public DecoderConfigItem CodeTable { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public DecoderConfigItem EasySearch { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public SimpleFieldLayout Layout { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public Tk5ListDetailConfig ListDetail { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public Tk5EditConfig Edit { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public Tk5ExtensionConfig Extension { get; private set; }

        public override string ToString()
        {
            string result;
            if (!string.IsNullOrEmpty(NickName))
                result = string.Format(ObjectUtil.SysCulture, "Add:[{0},{1}]", NickName, DataType);
            else if (!string.IsNullOrEmpty(FieldName))
                result = string.Format(ObjectUtil.SysCulture, "Add:[{0},{1}]", FieldName, DataType);
            else
                result = base.ToString();

            return result;
        }
    }
}
