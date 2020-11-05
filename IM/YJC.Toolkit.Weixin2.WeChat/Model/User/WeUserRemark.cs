using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Model.User
{
    public class WeUserRemark
    {
        [SimpleElement(NamingRule = NamingRule.Lower, Order = 10)]
        public string OpenId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 20)]
        public string Remark { get; set; }
    }
}