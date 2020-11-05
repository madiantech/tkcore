using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "将任意Toolkit对象输出为Json",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-10-17")]
    internal class JsonObjectPageMakerConfig : BaseObjectConfig
    {
        public override IPageMaker CreateObject(params object[] args)
        {
            return new JsonObjectPageMaker(this);
        }
    }
}
