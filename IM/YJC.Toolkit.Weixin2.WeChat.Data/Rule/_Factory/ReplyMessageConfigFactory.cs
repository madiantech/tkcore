using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    public sealed class ReplyMessageConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_weixin_ReplyMessage";
        private const string DESCRIPTION = "微信回复消息的配置插件工厂";

        public ReplyMessageConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}