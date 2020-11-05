using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class TreeNodeState
    {
        [SimpleAttribute(NamingRule = NamingRule.Camel, UseSourceType = true)]
        public bool Opened { get; set; }

        [SimpleAttribute(NamingRule = NamingRule.Camel, UseSourceType = true)]
        public bool Selected { get; set; }
    }
}