using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class MarcoTextPageMaker : IPageMaker
    {
        public MarcoTextPageMaker(MarcoConfigItem marco)
        {
            TkDebug.AssertArgumentNull(marco, "marco", null);

            Marco = marco;
            Encoding = ToolkitConst.UTF8;
            ContentType = ContentTypeConst.HTML;
        }

        internal MarcoTextPageMaker(MarcoTextPageMakerAttribute attribute)
        {
            Marco = new MarcoConfigItem(attribute.NeedParse, attribute.SqlInject, attribute.Value);
            Encoding = attribute.EncodingName.Value<Encoding>();
            ContentType = attribute.ContentType;
        }

        internal MarcoTextPageMaker(MarcoTextPageMakerConfig config)
        {
            Marco = config;
            Encoding = config.Encoding;
            ContentType = config.ContentType;
        }

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            string text = Expression.Execute(Marco, source, pageData, outputData);
            return new SimpleContent(ContentType ?? ContentTypeConst.HTML, text, Encoding);
        }

        #endregion

        public MarcoConfigItem Marco { get; private set; }

        public Encoding Encoding { get; set; }

        public string ContentType { get; set; }
    }
}
