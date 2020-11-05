using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [ObjectContext]
    internal class OverrideFieldConfig : BaseFieldConfig
    {
        [SimpleAttribute]
        public bool? IsEmpty { get; private set; }

        [SimpleAttribute]
        public int? Order { get; private set; }

        [SimpleAttribute]
        public ControlType? Control { get; private set; }

        [SimpleAttribute]
        public bool? PlaceHolder { get; private set; }

        [SimpleAttribute]
        public int? Length { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText DisplayName { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public HintData Hint { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public DecoderConfigItem CodeTable { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public DecoderConfigItem EasySearch { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public OverrideLayoutConfig Layout { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public OverrideListDetailConfig ListDetail { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public OverrideEditConfig Edit { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public OverrideExtensionConfig Extension { get; private set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(NickName) ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "Ovr:[{0}]", NickName);
        }
    }
}