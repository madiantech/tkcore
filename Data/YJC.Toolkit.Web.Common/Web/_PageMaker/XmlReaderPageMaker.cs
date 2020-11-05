using System.Xml;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Xml;

namespace YJC.Toolkit.Web
{
    internal class XmlReaderPageMaker : IPageMaker
    {
        public static readonly IPageMaker PageMaker = new XmlReaderPageMaker();

        private XmlReaderPageMaker()
        {
        }

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            TkDebug.AssertArgumentNull(outputData, "outputData", this);

            PageMakerUtil.AssertType(source, outputData, SourceOutputType.XmlReader, SourceOutputType.DataSet);
            using (XmlReader reader = PageMakerUtil.GetDataSetReader(outputData))
            {
                string xml = XmlUtil.GetXml(reader);
                return new SimpleContent(xml);
            }
        }

        #endregion

    }
}
