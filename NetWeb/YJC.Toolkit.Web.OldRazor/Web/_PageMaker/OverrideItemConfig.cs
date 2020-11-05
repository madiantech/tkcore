using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    class OverrideItemConfig : BaseRazorPageMakerConfig, IRegName
    {
        [SimpleAttribute]
        public PageStyleClass Style { get; private set; }

        [SimpleAttribute]
        public ContentDataType DataType { get; private set; }

        [SimpleAttribute]
        public PageStyle DestUrl { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool UseRetUrlFirst { get; set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public CustomUrlConfig CustomUrl { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText AlertMessage { get; private set; }


        #region IRegName 成员

        public string RegName
        {
            get
            {
                return Style.ToString();
            }
        }

        #endregion
    }
}
