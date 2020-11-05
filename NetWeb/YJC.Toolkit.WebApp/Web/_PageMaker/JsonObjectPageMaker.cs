using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class JsonObjectPageMaker : BaseObjectPageMaker
    {
        public JsonObjectPageMaker()
            : this(null, (WriteSettings)null)
        {
        }

        public JsonObjectPageMaker(string modelName, WriteSettings settings)
        {
            Add(PageMakerUtil.IsType(SourceOutputType.ToolkitObject),
                new InternalJsonObjectPageMaker(modelName, settings));
            Add(PageMakerUtil.IsType(SourceOutputType.String),
                new SourceOutputPageMaker(ContentTypeConst.JSON));
        }

        internal JsonObjectPageMaker(JsonObjectPageMakerAttribute attribute)
            : this(attribute.ModelName, (WriteSettings)null)
        {
            SetFormat(attribute);
        }

        internal JsonObjectPageMaker(JsonObjectPageMakerConfig config)
            : this(config.ModelName, config.WriteSettings)
        {
            SetFormat(config);
        }
    }
}
