using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "将任意Toolkit对象输出为Jsonp，允许跨域访问",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-01-19")]
    internal class JsonpObjectPageMakerConfig : IConfigCreator<IPageMaker>
    {
        [SimpleAttribute]
        public string ModelName { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public WriteSettings WriteSettings { get; protected set; }


        public IPageMaker CreateObject(params object[] args)
        {
            return new JsonpObjectPageMaker(this);
        }
    }
}
