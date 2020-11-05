using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "在Get时使用RazorPageMaker输出Html，在Post时使用PostPageMaker输出Json或者Xml",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-05-29")]
    internal class PostRazorPageMakerConfig : RazorPageMakerConfig
    {
        public override IPageMaker CreateObject(params object[] args)
        {
            IPageData pageData = ObjectUtil.ConfirmQueryObject<IPageData>(this, args);

            if (pageData.IsPost)
                return new PostPageMaker(DataType, DestUrl, CustomUrl)
                {
                    UseRetUrlFirst = UseRetUrlFirst,
                    AlertMessage = AlertMessage.ConvertToString()
                };
            else
                return new RazorPageMaker(this, pageData);
        }

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
    }
}
