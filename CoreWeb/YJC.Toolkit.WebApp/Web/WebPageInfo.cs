using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class WebPageInfo
    {
        public WebPageInfo(IPageData pageData, SessionGlobal sessionGbl, Uri retUrl, Uri selfUrl)
        {
            Url = new UrlInfo(retUrl, selfUrl);
            Info = new PageInfo(pageData, sessionGbl);
            QueryString = pageData.QueryString.ToDictionary();
        }

        [ObjectElement(LocalName = "URL")]
        public UrlInfo Url { get; private set; }

        [ObjectElement]
        public PageInfo Info { get; private set; }

        [Dictionary]
        public Dictionary<string, string> QueryString { get; private set; }

        public void AddToStringBuilder(StringBuilder builder)
        {
            var doc = this.CreateXDocument();
            string xml = string.Join(string.Empty, doc.Root.Elements());
            builder.Append(xml);
        }

        public void AddToXElement(XElement element)
        {
            var doc = this.CreateXDocument();
            var childs = doc.Root.Elements();
            foreach (var child in childs)
                element.Add(child);
        }

        public void AddToDynamic(dynamic data)
        {
            data.URL = Url;
            data.Info = Info;
            data.QueryString = QueryString;
        }
    }
}
