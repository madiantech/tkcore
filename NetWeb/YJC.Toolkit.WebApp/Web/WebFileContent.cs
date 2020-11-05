using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class WebFileContent : IContent
    {
        private readonly FileContent fContent;
        private readonly bool fUseCache;

        /// <summary>
        /// Initializes a new instance of the WebFileContent class.
        /// </summary>
        /// <param name="content"></param>
        public WebFileContent(FileContent content, bool useCache)
        {
            fContent = content;
            fUseCache = useCache;
        }

        public WebFileContent(FileContent content)
            : this(content, false)
        {
        }

        #region IContent 成员

        string IContent.ContentType
        {
            get
            {
                return fContent.ContentType;
            }
        }

        string IContent.Content
        {
            get
            {
                return null;
            }
        }

        Encoding IContent.ContentEncoding
        {
            get
            {
                return null;
            }
        }

        Dictionary<string, string> IContent.Headers
        {
            get
            {
                return null;
            }
        }

        Task IContent.WritePage(object resp)
        {
            HttpResponse response = resp.Convert<HttpResponse>();
            response.ClearContent();
            response.ClearHeaders();
            response.ContentType = fContent.ContentType;
            if (fUseCache)
            {
                response.Cache.SetCacheability(HttpCacheability.Public);
                response.Cache.SetExpires(DateTime.Now.AddYears(1));
            }
            if (fContent.IsWriteFileName)
            {
                string title = "filename=";
                HttpBrowserCapabilities browser = WebGlobalVariable.Request.Browser;
                string browserName = browser == null ? string.Empty : browser.Browser.ToLower(ObjectUtil.SysCulture);
                if (browserName == "firefox" || browserName == "opera")
                    title = "filename*=UTF-8''";
                string fileName = HttpUtility.UrlEncode(fContent.FileName, Encoding.UTF8);
                response.AddHeader("content-disposition", string.Format(ObjectUtil.SysCulture,
                    "{2}; {1}{0}", fileName, title, Disposition.ToString().ToLower(ObjectUtil.SysCulture)));
            }
            response.BinaryWrite(fContent.GetData());

            return null;
        }

        #endregion IContent 成员

        public ContentDisposition Disposition { get; set; }
    }
}