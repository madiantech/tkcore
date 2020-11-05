using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    internal class WeixinRuleXml : ToolkitConfig
    {
        [ObjectElement(NamespaceType.Toolkit)]
        [XmlPlugInItem]
        public RuleConfigItem Rule { get; private set; }
    }
}