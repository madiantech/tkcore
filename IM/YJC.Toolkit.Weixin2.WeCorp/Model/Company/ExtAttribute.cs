using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Company
{
    public class ExtAttribute
    {
        [SimpleElement(NamingRule = NamingRule.Camel, UseSourceType = true)]
        [TkTypeConverter(typeof(EnumIntTypeConverter), UseObjectType = true)]
        public ExtType Type { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [TagElement(NamingRule = NamingRule.Camel)]
        [SimpleElement(LocalName = "value")]
        public string Text { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public WebExtAttr Web { get; set; }

        [ObjectElement(NamingRule = NamingRule.Lower)]
        public MiniProgramExtAttr MiniProgram { get; set; }
    }
}