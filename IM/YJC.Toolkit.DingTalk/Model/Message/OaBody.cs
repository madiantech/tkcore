using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class OaBody
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Title { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<OaBodyForm> Form { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public OaBodyRich Rich { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Content { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Image { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int FileCount { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Author { get; set; }
    }
}