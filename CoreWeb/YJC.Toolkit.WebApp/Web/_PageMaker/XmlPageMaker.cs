using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class XmlPageMaker : BaseObjectPageMaker
    {
        public static readonly IPageMaker DEFAULT = new XmlPageMaker();

        public XmlPageMaker()
            : this(null, null)
        {
        }

        internal XmlPageMaker(XmlPageMakerConfig config)
            : this(config.WriteSettings, config.Root)
        {
            SetFormat(config);
        }

        internal XmlPageMaker(XmlPageMakerAttribute attribute)
            : this(null, attribute.CreateRootNode())
        {
            SetFormat(attribute);
        }

        public XmlPageMaker(WriteSettings settings, QName root)
        {
            Add(PageMakerUtil.IsType(SourceOutputType.XmlReader, SourceOutputType.DataSet),
                XmlReaderPageMaker.PageMaker);
            Add(PageMakerUtil.IsType(SourceOutputType.ToolkitObject),
                new XmlObjectPageMaker(settings, root));
            Add(PageMakerUtil.IsType(SourceOutputType.String),
                new SourceOutputPageMaker(ContentTypeConst.XML));
        }
    }
}
