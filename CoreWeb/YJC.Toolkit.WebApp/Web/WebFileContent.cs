using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
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

        Task IContent.WritePage(object response)
        {
            HttpResponse resp = response.Convert<HttpResponse>();
            resp.Clear();
            resp.ContentType = fContent.ContentType;
            if (fUseCache)
            {
                resp.GetTypedHeaders().CacheControl = new CacheControlHeaderValue
                {
                    Public = true,
                    MaxAge = TimeSpan.FromDays(1)
                };
            }

            if (fContent.IsWriteFileName)
            {
                string title = "filename=";
                var agentSvc = WebGlobalVariable.Context.RequestServices.GetService(
                    typeof(IUserAgentService)).Convert<IUserAgentService>();

                //HttpBrowserCapabilities browser = WebGlobalVariable.Request.Browser;
                string browserName = agentSvc.Agent.Browser.Name.ToLower(ObjectUtil.SysCulture);
                if (browserName == "firefox" || browserName == "opera")
                    title = "filename*=UTF-8''";
                string fileName = HttpUtility.UrlEncode(fContent.FileName, Encoding.UTF8);
                resp.Headers.Add("content-disposition",
                    $"{Disposition.ToString().ToLower(ObjectUtil.SysCulture)}; {title}{fileName}");
            }

            IFileInfo fileInfo = new WebFileInfo(fContent);
            return resp.SendFileAsync(fileInfo);
        }

        #endregion IContent 成员

        public ContentDisposition Disposition { get; set; }
    }
}