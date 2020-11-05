using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    public sealed class MatchRuleConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_weixin_MatchRule";
        private const string DESCRIPTION = "微信规则匹配的配置插件工厂";

        public MatchRuleConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}