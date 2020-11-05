using System;
using System.IO;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public static class UriUtil
    {
        public static Uri GetBaseUri(Uri uri)
        {
            TkDebug.AssertArgumentNull(uri, "uri", null);

            UriBuilder builder = new UriBuilder(uri) { Query = string.Empty };
            if (!string.IsNullOrEmpty(builder.Path))
                builder.Path = Path.GetDirectoryName(builder.Path) + "/";
            return builder.Uri;
        }

        public static string AppendQueryString(string baseUri, string queryString)
        {
            TkDebug.AssertArgumentNullOrEmpty(baseUri, "baseUri", null);

            if (string.IsNullOrEmpty(queryString))
                return baseUri;
            if (baseUri.Contains("?"))
            {
                if (!baseUri.EndsWith("&", StringComparison.Ordinal))
                    baseUri += "&";
            }
            else
                baseUri += "?";
            return baseUri + queryString;
        }

        public static Uri AppendQueryString(Uri uri, string queryString)
        {
            TkDebug.AssertArgumentNull(uri, "uri", null);

            if (string.IsNullOrEmpty(queryString))
                return uri;
            UriBuilder baseUri = new UriBuilder(uri);
            if (baseUri.Query != null && baseUri.Query.Length > 1)
                baseUri.Query = string.Format(ObjectUtil.SysCulture, "{0}{2}{1}", baseUri.Query.Substring(1),
                    queryString, baseUri.Query.EndsWith("&", StringComparison.Ordinal) ? string.Empty : "&");
            else
                baseUri.Query = queryString;

            return baseUri.Uri;
        }

        public static Uri CombineUri(Uri baseUri, string localPath)
        {
            TkDebug.AssertArgumentNull(baseUri, "baseUri", null);

            if (string.IsNullOrEmpty(localPath))
                return baseUri;
            return new Uri(baseUri, localPath);
        }

        public static Uri CombineUri(string hostName, string localPath)
        {
            return CombineUri(hostName, localPath, null);
        }

        public static Uri CombineUri(string hostName, string localPath, Uri defaultUri)
        {
            TkDebug.AssertArgumentNullOrEmpty(hostName, "hostName", null);
            TkDebug.AssertArgumentNullOrEmpty(localPath, "localPath", null);

            Uri baseUri = BaseAppSetting.Current.GetHost(hostName);
            if (baseUri == null)
                baseUri = defaultUri;
            if (baseUri == null)
                return new Uri(localPath);
            else
                return new Uri(baseUri, localPath);
        }

        public static string TextCombine(params string[] urlPaths)
        {
            if (urlPaths == null)
                return null;
            StringBuilder builder = new StringBuilder();
            int length = urlPaths.Length;
            for (int i = 0; i < length; i++)
            {
                string url = urlPaths[i];
                if (url == null)
                    continue;
                builder.Append(url);
                if (i == length - 1)
                    continue;
                bool endOperator = url.EndsWith("/", StringComparison.Ordinal);
                if (!endOperator)
                    builder.Append("/");
            }
            return builder.ToString();
        }
    }
}
