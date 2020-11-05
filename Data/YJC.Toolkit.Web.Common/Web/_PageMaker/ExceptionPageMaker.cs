using System.Xml.Linq;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Xml;

namespace YJC.Toolkit.Web
{
    [PageMaker(Author = "YJC", CreateDate = "2013-10-18",
        Description = "通过xslt转换的方式，显示Exception信息的PageMaker")]
    [InstancePlugIn, AlwaysCache]
    internal sealed class ExceptionPageMaker : IPageMaker
    {
        public static readonly IPageMaker Instance = new ExceptionPageMaker();

        private ExceptionPageMaker()
        {
        }

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            TkDebug.AssertArgumentNull(outputData, "outputData", this);

            PageMakerUtil.AssertType(source, outputData, SourceOutputType.ToolkitObject);
            ExceptionData data = outputData.Data.Convert<ExceptionData>();
            XDocument doc = data.CreateXDocument(null, ObjectUtil.WriteSettings, QName.ToolkitNoNS);

            string xsltFile = FileUtil.GetRealFileName(@"xslttemplate/bin/Exception.xslt",
                FilePathPosition.Xml);
            string content = XmlTransformUtil.Transform(doc.CreateReader(), xsltFile,
                null, TransformSetting.All);

            return new SimpleContent(ContentTypeConst.HTML, content);
        }

        #endregion IPageMaker 成员
    }
}