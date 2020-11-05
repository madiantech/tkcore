using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    internal class RuleConfigItem : IRegName, IAuthor, IXmlPlugInItem
    {
        public const string BASE_CLASS = "Rule";

        [SimpleAttribute(Required = true)]
        public string RegName { get; private set; }

        [SimpleAttribute]
        public string Description { get; private set; }

        [SimpleAttribute]
        public string Author { get; private set; }

        [SimpleAttribute]
        public string CreateDate { get; private set; }

        [TagElement(NamespaceType.Toolkit, Required = true)]
        [DynamicElement(MatchRuleConfigFactory.REG_NAME)]
        public IConfigCreator<RuleAttribute> Match { get; private set; }

        [TagElement(NamespaceType.Toolkit, Required = true)]
        [DynamicElement(ReplyMessageConfigFactory.REG_NAME)]
        public IConfigCreator<IRule> Reply { get; private set; }

        #region IXmlPlugInItem 成员

        public string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }

        #endregion IXmlPlugInItem 成员
    }
}