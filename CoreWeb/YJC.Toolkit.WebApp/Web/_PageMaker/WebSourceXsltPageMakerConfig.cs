using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "Get下使用XsltPageMaker，而Post下使用JsonObjectPageMaker",
        Author = "YJC", NamespaceType = NamespaceType.Toolkit, CreateDate = "2013-12-15")]
    class WebSourceXsltPageMakerConfig : IConfigCreator<IPageMaker>
    {
        private class WebSourceXsltPageMaker : CompositePageMaker
        {
            public WebSourceXsltPageMaker(WebSourceXsltPageMakerConfig config)
            {
                JsonObjectPageMaker objectMaker = new JsonObjectPageMaker
                {
                    IsEncrypt = WebAppSetting.WebCurrent.OutputEncrypt,
                    IsGZip = WebAppSetting.WebCurrent.OutputGZip
                };
                Add((source, pageData, outputData) => pageData.IsPost, objectMaker);
                string xsltFile = FileUtil.GetRealFileName(config.XsltFile, FilePathPosition.Xml);
                SimpleXsltPageMaker xsltMaker = new SimpleXsltPageMaker(xsltFile,
                    config.UseXsltArgs, ContentTypeConst.HTML, Encoding.UTF8);
                Add((source, pageData, outputData) => !pageData.IsPost, xsltMaker);
            }
        }

        #region IConfigCreator<IPageMaker> 成员

        public IPageMaker CreateObject(params object[] args)
        {
            return new WebSourceXsltPageMaker(this);
        }

        #endregion

        [SimpleAttribute]
        public bool UseXsltArgs { get; protected set; }

        [SimpleAttribute]
        public string XsltFile { get; protected set; }
    }
}
