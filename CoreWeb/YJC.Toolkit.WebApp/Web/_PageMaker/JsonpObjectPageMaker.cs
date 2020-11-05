using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class JsonpObjectPageMaker : JsonObjectPageMaker
    {
        public JsonpObjectPageMaker()
        {
        }

        public JsonpObjectPageMaker(string modelName, WriteSettings settings)
            : base(modelName, settings)
        {
        }

        internal JsonpObjectPageMaker(JsonpObjectPageMakerAttribute attribute)
            : this(attribute.ModelName, null)
        {
        }

        internal JsonpObjectPageMaker(JsonpObjectPageMakerConfig config)
            : this(config.ModelName, config.WriteSettings)
        {
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
