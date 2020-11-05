using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class SourceOutputPageMaker : IPageMaker
    {
        private readonly string fContentType;

        /// <summary>
        /// Initializes a new instance of the SourceOutputPageMaker class.
        /// </summary>
        public SourceOutputPageMaker()
            : this(ContentTypeConst.HTML)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SourceOutputPageMaker class.
        /// </summary>
        /// <param name="contentType"></param>
        public SourceOutputPageMaker(string contentType)
        {
            fContentType = contentType;
        }

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            TkDebug.AssertArgumentNull(outputData, "outputData", this);

            PageMakerUtil.AssertType(source, outputData, SourceOutputType.String);

            return new SimpleContent(fContentType, PageMakerUtil.GetString(outputData));
        }

        #endregion
    }
}
