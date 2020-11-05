using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public static class HtmlUtil
    {
        public static string ParseLinkUrl(IFieldValueProvider provider, string content)
        {
            string linkUrl = HtmlCommonUtil.ResolveString(provider, content);
            return WebUtil.ResolveUrl(linkUrl);
        }
    }
}