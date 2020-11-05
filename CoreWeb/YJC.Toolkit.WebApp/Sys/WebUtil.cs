using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public static class WebUtil
    {
        public const string SOURCE_INFO = "_tk_SourceInfo";

        public static string ResolveUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            if (IsAppRelative(url))
                return ToAbsolute(url);
            return url;
        }

        private static string ToAbsolute(string url)
        {
            return url.Substring(1);
        }

        private static bool IsAppRelative(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            return url.StartsWith("~/");
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

        //public static string GetQueryString(NameValueCollection queryString)
        //{
        //    if (queryString == null)
        //        return string.Empty;
        //    var queryItems = from key in queryString.AllKeys
        //                     where !IgnoreKey(key)
        //                     select string.Format(ObjectUtil.SysCulture, "{0}={1}",
        //                     key, HttpUtility.UrlEncode(queryString[key]));
        //    return string.Join("&", queryItems);
        //}

        internal static string GetQueryString(Uri url)
        {
            string query = url.Query;
            if (string.IsNullOrEmpty(query))
                return null;

            return query.Substring(1);
        }

        internal static string GetQueryString(HttpRequest request)
        {
            string query = request.QueryString.ToString();
            if (query != null && query.StartsWith('?'))
                return query.Substring(1);
            return query;
        }

        public static IModule CreateModule(this PageSourceInfo info)
        {
            TkDebug.AssertArgumentNull(info, "info", null);

            IModuleCreator moduleCreator = PlugInFactoryManager.CreateInstance<IModuleCreator>(
                ModuleCreatorPlugInFactory.REG_NAME, info.ModuleCreator);
            return moduleCreator.Create(info.Source);
        }

        public static PageSourceInfo CreateSourceInfo(HttpContext context)
        {
            TkDebug.AssertArgumentNull(context, nameof(context), null);

            var info = context.Items[SOURCE_INFO];
            if (info is PageSourceInfo sourceInfo)
                return sourceInfo;

            return null;
        }

        private static IHttpHandler CreateHanlderInfoCreator(PageSourceInfo info)
        {
            var handler = PlugInFactoryManager.CreateInstance<IHttpHandler>(
                HttpHandlerPlugInFactory.REG_NAME, info.Parser);
            return handler;
        }

        internal static string GetTemplateUrl(IPageStyle style, IPageData pageData)
        {
            var infoCreator = CreateHanlderInfoCreator(pageData.SourceInfo);

            return infoCreator.GetTemplateUrl(style, pageData);
        }

        internal static string GetTemplateUrl(PageStyle style, IPageData pageData)
        {
            var infoCreator = CreateHanlderInfoCreator(pageData.SourceInfo);

            return infoCreator.GetTemplateUrl((PageStyleClass)style, pageData);
        }

        public static Task WebDefaultHanlder(HttpContext context)
        {
            WebDefaultXmlConfig defaultConfig = WebGlobalVariable.WebCurrent.WebDefaultValue;
            IDefaultHandler handler = defaultConfig.DefaultHandler?.CreateObject() ?? EmptyDefaultHandlerConfig.Hanlder;

            return handler.Process(context);
        }
    }
}