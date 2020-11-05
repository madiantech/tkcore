using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public static class WebUtil
    {
        public static string ResolveUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            if (VirtualPathUtility.IsAppRelative(url))
                return VirtualPathUtility.ToAbsolute(url);
            return url;
        }

        private static bool IgnoreKey(string key)
        {
            return string.Compare(key, "RetURL", StringComparison.OrdinalIgnoreCase) == 0
                || string.Compare(key, "Source", StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static string GetQueryString(IQueryString queryString)
        {
            if (queryString == null)
                return string.Empty;
            var queryItems = from key in queryString.AllKeys
                             where !IgnoreKey(key)
                             select string.Format(ObjectUtil.SysCulture, "{0}={1}",
                             key, HttpUtility.UrlEncode(queryString[key]));
            return string.Join("&", queryItems);
        }

        internal static string GetQueryString(Uri url)
        {
            string query = url.Query;
            if (string.IsNullOrEmpty(query))
                return null;

            return query.Substring(1);
        }

        public static IModule CreateModule(this PageSourceInfo info)
        {
            TkDebug.AssertArgumentNull(info, "info", null);

            IModuleCreator moduleCreator = PlugInFactoryManager.CreateInstance<IModuleCreator>(
                ModuleCreatorPlugInFactory.REG_NAME, info.ModuleCreator);
            return moduleCreator.Create(info.Source);
        }

        private static IHandlerInfoCreator CreateHanlderInfoCreator(Uri url)
        {
            string extension = InternalUriUtil.GetUrlExtension(url).ToLower(ObjectUtil.SysCulture);
            IHandlerInfoCreator infoCreator = PlugInFactoryManager.CreateInstance<IHandlerInfoCreator>(
                HanlderInfoCreatorPlugInFactory.REG_NAME, extension);
            return infoCreator;
        }

        public static PageSourceInfo CreateSourceInfo(HttpRequest request)
        {
            TkDebug.AssertArgumentNull(request, "request", null);

            IHandlerInfoCreator infoCreator = CreateHanlderInfoCreator(request.Url);

            return infoCreator.CreateSourceInfo(request.Url, request.QueryString);
        }

        internal static string GetTemplateUrl(IPageStyle style, IPageData pageData)
        {
            var infoCreator = CreateHanlderInfoCreator(pageData.PageUrl);

            return infoCreator.GetTemplateUrl(style, pageData);
        }

        internal static string GetTemplateUrl(PageStyle style, IPageData pageData)
        {
            var infoCreator = CreateHanlderInfoCreator(pageData.PageUrl);

            return infoCreator.GetTemplateUrl((PageStyleClass)style, pageData);
        }
    }
}