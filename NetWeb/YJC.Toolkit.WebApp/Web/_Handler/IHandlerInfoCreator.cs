using System;
using System.Collections.Specialized;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public interface IHandlerInfoCreator
    {
        PageSourceInfo CreateSourceInfo(Uri url, NameValueCollection queryString);

        string GetTemplateUrl(IPageStyle style, IPageData pageData);
    }
}