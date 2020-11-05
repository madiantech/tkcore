using HtmlAgilityPack;
using System;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.HtmlExtension
{
    internal static class HtmlUtil
    {
        public const string CACHE_NAME = "_tk_HTML";

        private static void ReplaceNodes(HtmlDocument doc, string virtualPath,
            string elementName, string attrName)
        {
            var scripts = doc.DocumentNode.SelectNodes("//" + elementName);
            if (scripts == null)
                return;
            foreach (HtmlNode item in scripts)
            {
                string attrValue = item.GetAttributeValue(attrName, string.Empty);
                if (string.IsNullOrEmpty(attrValue))
                    continue;
                try
                {
                    Uri uri = new Uri(attrValue, UriKind.RelativeOrAbsolute);
                    if (!uri.IsAbsoluteUri)
                    {
                        if (!string.IsNullOrEmpty(virtualPath))
                            virtualPath += "/";
                        else
                            virtualPath = string.Empty;
                        string path = string.Format(ObjectUtil.SysCulture, "~/{0}{1}",
                            virtualPath, attrValue);
                        path = AppUtil.ResolveUrl(path);
                        item.SetAttributeValue(attrName, path);
                    }
                }
                catch
                {
                }
            }
        }

        private static void AddWebNode(HtmlDocument doc)
        {
            var body = doc.DocumentNode.SelectSingleNode("/html/body");
            try
            {
                if (body != null)
                    body.SetAttributeValue("data-webpath", BaseAppSetting.Current.AppVirtualPath);
            }
            catch
            {
            }
        }

        public static string GetCacheFileName(string fileName)
        {
            FileInfo file = new FileInfo(fileName);
            int sizeHash = file.Length.GetHashCode();
            int dateHash = file.LastWriteTime.GetHashCode();
            int hash = sizeHash ^ dateHash;
            string simpleFileName = Path.GetFileNameWithoutExtension(fileName);
            string result = string.Format("{0}.{1}.html", simpleFileName, hash);
            return result;
        }

        public static string GetCacheKey(string fileName, string virtualPath)
        {
            string cacheFile = GetCacheFileName(fileName);
            if (string.IsNullOrEmpty(virtualPath))
                return cacheFile;
            else
                return Path.Combine(virtualPath, cacheFile);
        }

        public static string GetCacheFile(string cacheFileName)
        {
            TkDebug.ThrowIfNoAppSetting();

            string path = Path.Combine(BaseAppSetting.Current.XmlPath, @"html\_temp", cacheFileName);
            return path;
        }

        public static string ReadHtmlFile(string fileName, string virtualPath, HtmlOption option)
        {
            if (option == null)
                option = HtmlOption.Default;

            HtmlDocument doc = new HtmlDocument();
            doc.Load(fileName);
            AddWebNode(doc);
            if (option.Script)
                ReplaceNodes(doc, virtualPath, "script", "src");
            if (option.Link)
                ReplaceNodes(doc, virtualPath, "link", "href");
            if (option.Img)
                ReplaceNodes(doc, virtualPath, "img", "src");
            if (option.A)
                ReplaceNodes(doc, virtualPath, "a", "href");
            TextWriter writer = new StringWriter();
            using (writer)
            {
                doc.Save(writer);
                return writer.ToString();
            }
        }
    }
}