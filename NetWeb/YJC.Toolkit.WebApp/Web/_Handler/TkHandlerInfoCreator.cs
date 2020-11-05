using System;
using System.Collections.Specialized;
using YJC.Toolkit.Cache;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [HanlderInfoCreator(RegName = "tkx", CreateDate = "2015-09-28", Author = "YJC",
        Description = DESCRIPTION)]
    [HanlderInfoCreator(RegName = "aspx", CreateDate = "2015-09-28", Author = "YJC",
        Description = DESCRIPTION)]
    [AlwaysCache, InstancePlugIn]
    internal sealed class TkHandlerInfoCreator : IHandlerInfoCreator
    {
        private const string DESCRIPTION = ".tkx和.aspx获取Handler信息的类";
        private static readonly IHandlerInfoCreator Instance = new TkHandlerInfoCreator();

        private TkHandlerInfoCreator()
        {
        }

        #region IHandlerInfoCreator 成员

        public PageSourceInfo CreateSourceInfo(Uri url, NameValueCollection queryString)
        {
            TkDebug.AssertArgumentNull(queryString, "queryString", null);

            string moduleCreator = queryString["useSource"];
            IPageStyle style = queryString["Style"].Value<PageStyleClass>();
            string source = queryString["Source"];

            return new PageSourceInfo(moduleCreator, style, source, true);
        }

        public string GetTemplateUrl(IPageStyle style, IPageData pageData)
        {
            if (style == null || pageData == null)
                return string.Empty;

            string source = pageData.SourceInfo.Source;
            PageStyle pageStyle = style.Style;
            switch (pageStyle)
            {
                case PageStyle.Custom:
                    string operation = string.IsNullOrEmpty(style.Operation) ? string.Empty :
                        "&Style=" + style.ToString();
                    return string.Format(ObjectUtil.SysCulture, "Library/WebModuleContentPage.{1}?Source={2}{0}",
                        operation, pageData.PageExtension, source);
                case PageStyle.Insert:
                case PageStyle.List:
                case PageStyle.Update:
                case PageStyle.Delete:
                case PageStyle.Detail:
                    return string.Format(ObjectUtil.SysCulture, "Library/Web{0}XmlPage.{1}?Source={2}",
                        pageStyle, pageData.PageExtension, source);
                default:
                    TkDebug.ThrowImpossibleCode(this);
                    return null;
            }
        }

        #endregion
    }
}
