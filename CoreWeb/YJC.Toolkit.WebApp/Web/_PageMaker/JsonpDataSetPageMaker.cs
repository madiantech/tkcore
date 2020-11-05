using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class JsonpDataSetPageMaker : CompositePageMaker
    {
        public JsonpDataSetPageMaker()
        {
            Add(PageMakerUtil.IsType(SourceOutputType.XmlReader, SourceOutputType.DataSet),
               JsonDataSetXmlReaderPageMaker.PageMaker);
            Add(PageMakerUtil.IsType(SourceOutputType.String), JsonDataSetTextPageMaker.PageMaker);
        }

        protected override IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            IContent content = base.WritePage(source, pageData, outputData);
            string param = pageData.QueryString["jsonpcallback"].Value<string>("jsonpcallback");
            string text = string.Format(ObjectUtil.SysCulture, "{0}({1})", param, content.Content);
            IContent result = new SimpleContent(NetUtil.GetContentType(".js"), text, content.ContentEncoding);
            return result;
        }
    }
}
