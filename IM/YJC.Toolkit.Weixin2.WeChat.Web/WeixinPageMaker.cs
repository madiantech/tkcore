using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;
using YJC.Toolkit.Weixin;

namespace YJC.Toolkit.WeChat.Web
{
    internal class WeixinPageMaker : CompositePageMaker
    {
        public WeixinPageMaker()
        {
            Add((source, input, output) =>
                !input.IsPost || (input.IsPost && output.OutputType == SourceOutputType.String),
                new SourceOutputPageMaker());
            WriteSettings setting = new WriteSettings
            {
                OmitHead = true
            };
            XmlObjectPageMaker xmlMaker = new XmlObjectPageMaker(setting, WeConst.ROOT)
            {
                ModelName = WeConst.XML_MODE
            };
            Add((source, input, output) =>
                input.IsPost && output.OutputType == SourceOutputType.ToolkitObject, xmlMaker);
        }
    }
}