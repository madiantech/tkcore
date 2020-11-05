using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public static partial class HtmlUtil
    {
        public static string GetDynamicPageId(dynamic callerInfo)
        {
            try
            {
                PageInfo info = callerInfo.Info;
                return string.Format(ObjectUtil.SysCulture, "Web{0}XmlPage", info.Style);
            }
            catch
            {
                return "WebPage";
            }
        }

        public static string GetDynamicRetUrl(dynamic callerInfo)
        {
            string homePage = WebAppSetting.WebCurrent.HomePath;
            if (callerInfo == null)
                return homePage;

            UrlInfo url = callerInfo.URL;
            string retUrl = url != null ? url.RetUrl : null;
            if (string.IsNullOrEmpty(retUrl))
                return homePage;
            return WebUtil.ResolveUrl(retUrl);
        }
    }
}
