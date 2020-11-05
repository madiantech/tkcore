using System.Collections.Generic;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class AppMessageResult : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(CommaStringListTypeConverter))]
        public List<string> InvalidUserIdList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(CommaStringListTypeConverter))]
        public List<string> ForbiddenUserIdList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(CommaStringListTypeConverter))]
        public List<string> FailedUserIdList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(CommaStringListTypeConverter))]
        public List<string> ReadUserIdList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(CommaStringListTypeConverter))]
        public List<string> UnreadUserIdList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(CommaStringListTypeConverter))]
        public List<string> InvalidDeptIdList { get; set; }
    }
}