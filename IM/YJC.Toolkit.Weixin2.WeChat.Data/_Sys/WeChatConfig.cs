using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat
{
    internal class WeChatConfig
    {
        [SimpleAttribute(Required = true)]
        public string AppId { get; private set; }

        [SimpleAttribute(Required = true)]
        public string AppSecret { get; protected set; }

        [SimpleAttribute(Required = true)]
        public string Token { get; protected set; }
        [SimpleAttribute(Required = true)]
        public string OpenId { get; private set; }

        [SimpleAttribute]
        public MessageMode MessageMode { get; private set; }

        [SimpleAttribute]
        public string EncodingAESKey { get; protected set; }

        [SimpleAttribute]
        public bool LogRawMessage { get; private set; }

    }
}
