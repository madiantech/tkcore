using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class MarkdownCorpMessage : BaseCorpMessage
    {
        public MarkdownCorpMessage(int agentId, string content)
            : base(agentId, MessageType.Markdown)
        {
            TkDebug.AssertArgumentNullOrEmpty(content, nameof(content), null);

            Content = content;
        }

        public MarkdownCorpMessage(string appName, string content)
            : base(appName, MessageType.Markdown)
        {
            TkDebug.AssertArgumentNullOrEmpty(content, nameof(content), null);

            Content = content;
        }

        [TagElement(LocalName = "markdown", Order = 50)]
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Content { get; set; }
    }
}