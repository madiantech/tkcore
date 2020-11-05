using System.Xml;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Xml;

namespace YJC.Toolkit.Web
{
    internal sealed class JsonDataSetXmlReaderPageMaker : IPageMaker
    {
        public static readonly IPageMaker PageMaker = new JsonDataSetXmlReaderPageMaker();

        /// <summary>
        /// Initializes a new instance of the JsonXmlReaderPageMaker class.
        /// </summary>
        private JsonDataSetXmlReaderPageMaker()
        {
        }

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            TkDebug.AssertArgumentNull(outputData, "outputData", this);

            PageMakerUtil.AssertType(source, outputData,
                SourceOutputType.XmlReader, SourceOutputType.DataSet);

            using (XmlReader reader = PageMakerUtil.GetDataSetReader(outputData))
            {
                string xml = XmlUtil.GetJson(reader);
                return new SimpleContent(ContentTypeConst.JSON, xml);
            }
        }

        #endregion
    }
}
