using System.IO;
using System.Xml;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Xml;

namespace YJC.Toolkit.Web
{
    internal sealed class JsonDataSetTextPageMaker : IPageMaker
    {
        public static readonly IPageMaker PageMaker = new JsonDataSetTextPageMaker();

        /// <summary>
        /// Initializes a new instance of the JsonTextPageMaker class.
        /// </summary>
        private JsonDataSetTextPageMaker()
        {
        }

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            TkDebug.AssertArgumentNull(outputData, "outputData", this);

            PageMakerUtil.AssertType(source, outputData, SourceOutputType.String);
            StringReader strReader = new StringReader(PageMakerUtil.GetString(outputData));
            using (XmlReader reader = XmlReader.Create(strReader, new XmlReaderSettings { CloseInput = true }))
            {
                string xml = XmlUtil.GetJson(reader);
                return new SimpleContent(ContentTypeConst.JSON, xml);
            }
        }

        #endregion
    }
}
