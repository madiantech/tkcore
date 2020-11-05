using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class SendMessageResult : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Lower)]
        [TkTypeConverter(typeof(StringListTypeConverter))]
        public List<string> InvalidUser { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        [TkTypeConverter(typeof(StringListTypeConverter))]
        public List<string> InvalidParty { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        [TkTypeConverter(typeof(StringListTypeConverter))]
        public List<string> InvalidTag { get; set; }
    }
}