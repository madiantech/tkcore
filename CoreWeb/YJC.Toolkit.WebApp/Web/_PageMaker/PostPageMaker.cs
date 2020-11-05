using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class PostPageMaker : IPageMaker
    {
        public PostPageMaker(ContentDataType dataType, PageStyle destUrl, CustomUrlConfig customUrl)
        {
            SetProperties(dataType, destUrl, customUrl);
            UseRetUrlFirst = true;
        }

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            IPageMaker pageMaker = null;
            switch (DataType)
            {
                case ContentDataType.Json:
                    pageMaker = new JsonObjectPageMaker();
                    break;

                case ContentDataType.Xml:
                    pageMaker = new XmlObjectPageMaker(null, null);
                    break;

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    break;
            }
            WebErrorResult error = outputData.Data as WebErrorResult;
            if (error != null)
                return pageMaker.WritePage(source, pageData, outputData);
            else
            {
                string url = null;
                if (UseRetUrlFirst)
                    url = pageData.QueryString["RetURL"];
                if (string.IsNullOrEmpty(url))
                    url = GetDefaultUrl(source, pageData, outputData);
                WebSuccessResult success = new WebSuccessResult(url)
                {
                    AlertMessage = AlertMessage
                };
                OutputData newData = OutputData.CreateToolkitObject(success);
                return pageMaker.WritePage(source, pageData, newData);
            }
        }

        #endregion IPageMaker 成员

        public ContentDataType DataType { get; private set; }

        public PageStyle DestUrl { get; private set; }

        public CustomUrlConfig CustomUrl { get; private set; }

        public bool UseRetUrlFirst { get; set; }

        public string AlertMessage { get; set; }

        protected virtual string GetDefaultUrl(ISource source, IPageData pageData, OutputData outputData)
        {
            string url = string.Empty;
            KeyData keyData = outputData.Data.Convert<KeyData>();
            switch (DestUrl)
            {
                case PageStyle.Custom:
                    TkDebug.AssertNotNull(CustomUrl,
                        "配置了DestUrl为Custom，却没有配置CustomUrl", this);
                    url = Expression.Execute(CustomUrl, source);
                    url = AppUtil.ResolveUrl(url);
                    if (CustomUrl.UseKeyData)
                        url = UriUtil.AppendQueryString(url, keyData.ToString());
                    break;

                case PageStyle.Insert:
                case PageStyle.List:
                    url = WebUtil.GetTemplateUrl(DestUrl, pageData).AppVirutalPath();
                    break;

                case PageStyle.Update:
                case PageStyle.Delete:
                case PageStyle.Detail:
                    url = WebUtil.GetTemplateUrl(DestUrl, pageData);
                    url = UriUtil.AppendQueryString(url, keyData.ToString()).AppVirutalPath();
                    break;

                default:
                    TkDebug.ThrowImpossibleCode(this);
                    break;
            }

            return url;
        }

        internal void SetProperties(ContentDataType dataType, PageStyle destUrl, CustomUrlConfig customUrl)
        {
            DataType = dataType;
            DestUrl = destUrl;
            CustomUrl = customUrl;
        }
    }
}