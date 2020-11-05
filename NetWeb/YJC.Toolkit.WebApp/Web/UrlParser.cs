using System;
using System.IO;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class UrlParser
    {
        private UrlParser(string url)
        {
            string[] urlPart = url.Split('/');
            TkDebug.Assert(urlPart.Length >= 2, "url链接错误，当前url是" + url, null);

            string lastPart = Path.GetFileNameWithoutExtension(urlPart[urlPart.Length - 1]);
            string source;
            bool isContent;
            if (lastPart.StartsWith("~", StringComparison.Ordinal))
            {
                isContent = false;
                source = GetSource(lastPart.Substring(1));
            }
            else
            {
                source = GetSource(lastPart);
                isContent = true;
            }
            IPageStyle style;
            if (urlPart.Length - 2 >= 1)
                style = urlPart[urlPart.Length - 2].Value<PageStyleClass>();
            else
                style = null;
            string moduleCreator;
            if (urlPart.Length - 3 >= 1)
                moduleCreator = urlPart[urlPart.Length - 3];
            else
                moduleCreator = null;
            Info = new PageSourceInfo(moduleCreator, style, source, isContent);
        }

        public PageSourceInfo Info { get; private set; }

        private static string GetSource(string source)
        {
            return source.Replace('_', '/');
        }

        public static UrlParser Create(string url)
        {
            TkDebug.AssertArgumentNullOrEmpty(url, "url", null);
            TkDebug.AssertArgument(url.StartsWith("~/", StringComparison.Ordinal)
                || url.StartsWith("/", StringComparison.Ordinal), "url", "url必须是~/或/起始", null);

            return new UrlParser(url);
        }
    }
}
