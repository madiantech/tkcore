using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "将DataSet或者类DataSet的Xml输出为Xml",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-10-16")]
    internal class XmlPageMakerConfig : BaseObjectConfig
    {
        [SimpleAttribute]
        public QName Root { get; private set; }

        public override IPageMaker CreateObject(params object[] args)
        {
            return new XmlPageMaker(this);
        }
    }
}
