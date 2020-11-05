using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class JsonDataSetPageMaker : BaseObjectPageMaker
    {
        /// <summary>
        /// Initializes a new instance of the JsonPageMaker class.
        /// </summary>
        public JsonDataSetPageMaker()
        {
            Add(PageMakerUtil.IsType(SourceOutputType.XmlReader, SourceOutputType.DataSet),
                JsonDataSetXmlReaderPageMaker.PageMaker);
            Add(PageMakerUtil.IsType(SourceOutputType.String), JsonDataSetTextPageMaker.PageMaker);
        }

        internal JsonDataSetPageMaker(JsonDataSetPageMakerAttribute attribute)
            : this()
        {
            SetFormat(attribute);
        }

        internal JsonDataSetPageMaker(JsonDataSetPageMakerConfig config)
            : this()
        {
            SetFormat(config);
        }
    }
}
