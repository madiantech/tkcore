using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "直接将配置的文件内容输出的PageMaker",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-08-13")]
    internal class FilePageMakerConfig : IConfigCreator<IPageMaker>
    {
        [SimpleAttribute(DefaultValue = ContentTypeConst.HTML)]
        public string ContentType { get; protected set; }

        [SimpleAttribute]
        public string FileName { get; protected set; }

        [SimpleAttribute(DefaultValue = "utf-8")]
        public Encoding Encoding { get; protected set; }

        [SimpleAttribute(DefaultValue = "utf-8")]
        public Encoding FileEncoding { get; protected set; }

        [SimpleAttribute(DefaultValue = FilePathPosition.Xml)]
        public FilePathPosition Position { get; private set; }

        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            return new FilePageMaker(this);
        }

        #endregion
    }
}
