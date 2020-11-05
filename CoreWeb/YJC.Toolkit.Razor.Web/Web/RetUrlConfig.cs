using System;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class RetUrlConfig
    {
        public RetUrlConfig()
        {
        }

        public RetUrlConfig(PageStyle style, bool useRetUrlFirst, MarcoConfigItem customUrl)
        {
            Style = style;
            UseRetUrlFirst = useRetUrlFirst;
            CustomUrl = customUrl;
        }

        [SimpleAttribute]
        public PageStyle? Style { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool UseRetUrlFirst { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem CustomUrl { get; private set; }

        private static Uri GetDefaultStyleUrl(PageStyle style, IPageData input)
        {
            string result;
            string queryString;
            switch (style)
            {
                case PageStyle.Insert:
                case PageStyle.List:
                    result = "~/" + WebUtil.GetTemplateUrl(style, input);
                    break;

                case PageStyle.Update:
                case PageStyle.Detail:
                    queryString = WebUtil.GetQueryString(input.QueryString);
                    result = "~/" + WebUtil.GetTemplateUrl(style, input);
                    result = UriUtil.AppendQueryString(result, queryString);
                    break;

                default:
                    return null;
            }
            return new Uri(WebUtil.ResolveUrl(result), UriKind.RelativeOrAbsolute);
        }

        public static Uri GetRetUrl(RetUrlConfig config, IPageData input)
        {
            if (config == null || config.Style == null)
            {
                string retUrl = input.QueryString["RetURL"];
                if (!string.IsNullOrEmpty(retUrl))
                    return new Uri(retUrl, UriKind.RelativeOrAbsolute);
                return GetDefaultStyleUrl(PageStyle.List, input);
            }
            else
            {
                if (config.UseRetUrlFirst)
                {
                    string retUrl = input.QueryString["RetURL"];
                    if (!string.IsNullOrEmpty(retUrl))
                        return new Uri(retUrl, UriKind.RelativeOrAbsolute);
                }
                switch (config.Style.Value)
                {
                    case PageStyle.Custom:
                        if (config.CustomUrl != null)
                        {
                            string url = WebUtil.ResolveUrl(Expression.Execute(config.CustomUrl));
                            return new Uri(url, UriKind.RelativeOrAbsolute);
                        }
                        break;

                    case PageStyle.Insert:
                    case PageStyle.List:
                    case PageStyle.Update:
                    case PageStyle.Delete:
                    case PageStyle.Detail:
                        return GetDefaultStyleUrl(config.Style.Value, input);
                }
            }
            return null;
        }
    }
}