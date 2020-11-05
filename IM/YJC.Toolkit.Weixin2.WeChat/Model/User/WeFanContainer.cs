using System.Collections.Generic;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Model.User
{
    public class WeFanContainer : BaseResult
    {
        internal WeFanContainer()
        {
        }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 10)]
        public int Total { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 20)]
        public int Count { get; private set; }

        [TagElement(LocalName = "data", Order = 30)]
        [SimpleElement(LocalName = "openid", IsMultiple = true)]
        public List<string> OpenIds { get; private set; }

        [SimpleElement(LocalName = "next_openid", Order = 40)]
        public string NextOpenId { get; private set; }
    }
}