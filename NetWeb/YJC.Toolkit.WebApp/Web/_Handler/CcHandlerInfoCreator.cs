using System;
using System.Collections.Specialized;
using System.Web;
using YJC.Toolkit.Cache;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [HanlderInfoCreator(RegName = "c", CreateDate = "2015-09-28", Author = "YJC",
       Description = ".c获取Handler信息的类")]
    [AlwaysCache, InstancePlugIn]
    internal class CcHandlerInfoCreator : IHandlerInfoCreator
    {
        private static readonly IHandlerInfoCreator Instance = new CcHandlerInfoCreator();

        private CcHandlerInfoCreator()
        {
        }

        #region IHandlerInfoCreator 成员

        public PageSourceInfo CreateSourceInfo(Uri url, NameValueCollection queryString)
        {
            TkDebug.AssertArgumentNull(url, "url", null);

            string path = VirtualPathUtility.ToAppRelative(url.AbsolutePath);
            UrlParser parser = UrlParser.Create(path);
            return parser.Info;
        }

        public string GetTemplateUrl(IPageStyle style, IPageData pageData)
        {
            if (style == null || pageData == null)
                return string.Empty;

            string source = pageData.SourceInfo.CcSource;
            PageStyle pageStyle = style.Style;
            string styleString = pageStyle.ToString().ToLower(ObjectUtil.SysCulture);
            switch (pageStyle)
            {
                case PageStyle.Custom:
                    if (string.IsNullOrEmpty(style.Operation))
                        return source + ".c";
                    else
                        return string.Format(ObjectUtil.SysCulture, "{0}/{1}.c", style, source);
                case PageStyle.Insert:
                case PageStyle.List:
                case PageStyle.Update:
                case PageStyle.Delete:
                case PageStyle.Detail:
                    return string.Format(ObjectUtil.SysCulture, "{0}/{1}.c", styleString, source);
                default:
                    TkDebug.ThrowImpossibleCode(this);
                    return null;
            }
        }

        #endregion
    }
}
