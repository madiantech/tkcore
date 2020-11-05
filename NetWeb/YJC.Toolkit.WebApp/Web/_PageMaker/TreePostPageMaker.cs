using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class TreePostPageMaker : PostPageMaker
    {
        public TreePostPageMaker(ContentDataType dataType, PageStyle destUrl, CustomUrlConfig customUrl)
            : base(dataType, destUrl, customUrl)
        {
        }

        protected override string GetDefaultUrl(ISource source, IPageData pageData, OutputData outputData)
        {
            string url = string.Empty;
            KeyData keyData = outputData.Data.Convert<KeyData>();
            string pageSource = pageData.SourceInfo.CcSource;
            switch (DestUrl)
            {
                case PageStyle.Custom:
                    TkDebug.AssertNotNull(CustomUrl,
                        "配置了DestUrl为Custom，却没有配置CustomUrl", this);
                    url = Expression.Execute(CustomUrl, source);
                    url = WebUtil.ResolveUrl(url);
                    if (CustomUrl.UseKeyData)
                        url = UriUtil.AppendQueryString(url, keyData.ToString());
                    break;
                case PageStyle.Insert:
                case PageStyle.Update:
                case PageStyle.Delete:
                    url = WebUtil.GetTemplateUrl(DestUrl, pageData);
                    url = UriUtil.AppendQueryString(url, keyData.ToString()).AppVirutalPath();
                    break;
                case PageStyle.Detail:
                case PageStyle.List:
                    string initValue = keyData.IsSingleValue ? keyData.SingleValue : string.Empty;
                    url = WebUtil.GetTemplateUrl((PageStyleClass)string.Empty, pageData);
                    url = UriUtil.AppendQueryString(url, "InitValue=" + initValue).AppVirutalPath();
                    break;
                default:
                    TkDebug.ThrowImpossibleCode(this);
                    break;
            }

            return url;
        }
    }
}
