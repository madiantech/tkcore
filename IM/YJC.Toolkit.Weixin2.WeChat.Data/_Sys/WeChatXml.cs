using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat
{
    internal class WeChatXml
    {
        [ObjectElement(NamespaceType.Toolkit)]
        public WeChatConfig WeChat { get; private set; }
    }
}
