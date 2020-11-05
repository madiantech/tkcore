using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class XmlObjectPageMaker : IPageMaker
    {
        private readonly WriteSettings fSettings;
        private readonly QName fRoot;

        public XmlObjectPageMaker(WriteSettings settings, QName root)
        {
            fSettings = settings ?? ObjectUtil.WriteSettings;
            fRoot = root ?? QName.Get(ToolkitConst.TOOLKIT);
        }

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            TkDebug.AssertArgumentNull(outputData, "outputData", this);

            PageMakerUtil.AssertType(source, outputData, SourceOutputType.ToolkitObject);
            string xml = outputData.Data.WriteXml(ModelName, fSettings, fRoot);
            return new SimpleContent(ContentTypeConst.XML, xml);
        }

        #endregion IPageMaker 成员

        public string ModelName { get; set; }
    }
}