using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "将定义的宏文本输出的PageMaker",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-08-13")]
    internal class MarcoTextPageMakerConfig : MarcoConfigItem, IConfigCreator<IPageMaker>
    {
        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            return new MarcoTextPageMaker(this);
        }

        #endregion

        [SimpleAttribute(DefaultValue = "UTF-8")]
        public Encoding Encoding { get; private set; }

        [SimpleAttribute(DefaultValue = ContentTypeConst.HTML)]
        public string ContentType { get; private set; }
    }
}
