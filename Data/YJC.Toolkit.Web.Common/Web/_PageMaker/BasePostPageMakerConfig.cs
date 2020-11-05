using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal abstract class BasePostPageMakerConfig
    {
        protected BasePostPageMakerConfig()
        {
        }

        [SimpleAttribute]
        public ContentDataType DataType { get; protected set; }

        [SimpleAttribute]
        public PageStyle DestUrl { get; protected set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool UseRetUrlFirst { get; set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public CustomUrlConfig CustomUrl { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText AlertMessage { get; protected set; }
    }
}