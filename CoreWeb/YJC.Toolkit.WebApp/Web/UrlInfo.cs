using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class UrlInfo
    {
        internal UrlInfo(Uri retUrl, Uri selfUrl)
        {
            DRetUrl = retUrl == null ? string.Empty :
                (retUrl.IsAbsoluteUri ? retUrl.PathAndQuery : retUrl.OriginalString);
            DSelfUrl = ParseUrl(selfUrl);
        }

        [SimpleElement(LocalName = "RetURL")]
        public string RetUrl
        {
            get
            {
                return HttpUtility.UrlEncode(DRetUrl, Encoding.UTF8);
            }
            set
            {
            }
        }

        [SimpleElement(LocalName = "SelfURL")]
        public string SelfUrl
        {
            get
            {
                return HttpUtility.UrlEncode(DSelfUrl, Encoding.UTF8);
            }
            set
            {
            }
        }

        [SimpleElement(LocalName = "DRetURL")]
        public string DRetUrl { get; private set; }

        [SimpleElement(LocalName = "DSelfURL")]
        public string DSelfUrl { get; private set; }

        private static string ParseUrl(Uri url)
        {
            if (url == null)
                return string.Empty;
            UriBuilder builder = new UriBuilder(url);
            if (string.IsNullOrEmpty(builder.Query))
                return url.PathAndQuery;
            if (builder.Port == 80)
                builder.Port = -1;
            Dictionary<string, string> query = ParseQueryString(builder.Query);
            if (query != null)
                builder.Query = string.Join("&",
                    from item in query
                    select string.Format(ObjectUtil.SysCulture, "{0}={1}", item.Key, item.Value));
            return builder.Uri.PathAndQuery;
        }

        private static Dictionary<string, string> ParseQueryString(string s)
        {
            int length = (s != null) ? s.Length : 0;
            if (length == 0)
                return null;
            Dictionary<string, string> result = new Dictionary<string, string>();
            int start = s[0] == '?' ? 1 : 0;
            for (int i = start; i < length; i++)
            {
                int startIndex = i;
                int equalIndex = -1;
                while (i < length)
                {
                    char ch = s[i];
                    if (ch == '=')
                    {
                        if (equalIndex < 0)
                            equalIndex = i;
                    }
                    else if (ch == '&')
                        break;
                    i++;
                }
                string name = null;
                string value = null;
                if (equalIndex >= 0)
                {
                    name = s.Substring(startIndex, equalIndex - startIndex);
                    value = s.Substring(equalIndex + 1, (i - equalIndex) - 1);
                }
                else
                    value = s.Substring(startIndex, i - startIndex);
                if (string.IsNullOrEmpty(name))
                    continue;
                if (string.Compare(name, "RetURL", StringComparison.CurrentCultureIgnoreCase) == 0)
                    result.Add(name, HttpUtility.UrlEncode(value));
                else
                    result.Add(name, value);
                if ((i == (length - 1)) && (s[i] == '&'))
                {
                    //base.Add(null, string.Empty);
                }
            }

            return result;
        }
    }
}