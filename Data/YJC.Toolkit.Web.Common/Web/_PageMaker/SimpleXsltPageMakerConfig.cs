using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "将结果进行xslt转换后再输出", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", CreateDate = "2013-10-16")]
    internal class SimpleXsltPageMakerConfig : IConfigCreator<IPageMaker>
    {
        [SimpleAttribute]
        public bool UseXsltArgs { get; protected set; }

        [SimpleAttribute(DefaultValue = ContentTypeConst.HTML)]
        public string ContentType { get; protected set; }

        [SimpleAttribute]
        public string XsltFile { get; protected set; }

        [SimpleAttribute(DefaultValue = "utf-8")]
        public Encoding Encoding { get; protected set; }

        [SimpleAttribute(DefaultValue = FilePathPosition.Xml)]
        public FilePathPosition Position { get; private set; }

        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            string path = FileUtil.GetRealFileName(XsltFile, Position);
            return new SimpleXsltPageMaker(path, UseXsltArgs, ContentType, Encoding);
        }

        #endregion
    }
}
