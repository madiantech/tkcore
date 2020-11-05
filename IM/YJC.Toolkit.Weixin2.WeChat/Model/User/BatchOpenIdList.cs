using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Model.User
{
    public class BatchOpenIdList
    {
        public class OpenIdItem
        {
            [SimpleAttribute(NamingRule = NamingRule.Lower)]
            public string OpenId { get; internal set; }

            [SimpleAttribute(NamingRule = NamingRule.Lower)]
            public string Lang { get; internal set; }
        }

        public BatchOpenIdList(params string[] openIds)
            : this("zh-CN", openIds)
        {
        }

        public BatchOpenIdList(string lang, IEnumerable<string> openIds)
        {
            TkDebug.AssertArgumentNullOrEmpty(lang, "lang", null);
            TkDebug.AssertEnumerableArgumentNullOrEmpty(openIds, "openIds", null);

            UserList = new List<OpenIdItem>();
            foreach (var openId in openIds)
            {
                OpenIdItem item = new OpenIdItem
                {
                    OpenId = openId,
                    Lang = lang
                };
                UserList.Add(item);
            }
        }

        [ObjectElement(IsMultiple = true, NamingRule = NamingRule.UnderLineLower)]
        public List<OpenIdItem> UserList { get; private set; }
    }
}