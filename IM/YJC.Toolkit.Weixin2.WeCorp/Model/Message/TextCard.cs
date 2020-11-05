using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class TextCard
    {
        public TextCard(string title, string description, string url)
        {
            TkDebug.AssertArgumentNullOrEmpty(title, nameof(title), null);
            TkDebug.AssertArgumentNullOrEmpty(description, nameof(description), null);
            TkDebug.AssertArgumentNullOrEmpty(url, nameof(url), null);

            Title = title;
            Description = description;
            Url = url;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        //视频消息的标题
        public string Title { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        //视频消息的描述
        public string Description { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        //点击后跳转的链接
        public string Url { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        //按钮文字，默认为“详情”
        public string BtnTxt { get; set; }
    }
}