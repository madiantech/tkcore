using System.Collections.Generic;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class MiniProgramNotice
    {
        public MiniProgramNotice(string appId, string title)
        {
            TkDebug.AssertArgumentNullOrEmpty(appId, nameof(appId), null);
            TkDebug.AssertArgumentNullOrEmpty(title, nameof(title), null);

            AppId = appId;
            Title = title;
        }

        [SimpleElement(NamingRule = NamingRule.Lower)]//小程序appid，必须是与当前小程序应用关联的小程序
        public string AppId { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]//点击消息卡片后的小程序页面，仅限本小程序内的页面。该字段不填则消息点击后不跳转。
        public string Page { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]//消息标题，长度限制4-12个汉字
        public string Title { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]//消息描述，长度限制4-12个汉字
        public string Description { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, UseSourceType = true)]//是否放大第一个content_item
        public bool EmphasisFirstItem { get; set; }

        [ObjectElement(LocalName = "content_item", IsMultiple = true)]//消息内容键值对，最多允许10个item
        public List<YJC.Toolkit.Weixin.KeyValuePair> ContentItems { get; set; }
    }
}