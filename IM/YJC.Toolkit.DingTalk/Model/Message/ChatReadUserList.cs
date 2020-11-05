using System.Collections.Generic;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class ChatReadUserList : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string NextCursor { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<string> ReadUserIdList { get; set; }
    }
}