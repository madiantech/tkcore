using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(NamespaceType = NamespaceType.Toolkit, Description = "将内容直接输出",
        Author = "YJC", CreateDate = "2013-10-11")]
    internal class SourceOutputPageMakerConfig : IConfigCreator<IPageMaker>
    {
        [SimpleAttribute(DefaultValue = ContentTypeConst.HTML)]
        public string ContentType { get; private set; }

        [SimpleAttribute(DefaultValue = "utf-8")]
        public Encoding Encoding { get; protected set; }

        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            return new SourceOutputPageMaker(ContentType);
        }

        #endregion
    }
}
