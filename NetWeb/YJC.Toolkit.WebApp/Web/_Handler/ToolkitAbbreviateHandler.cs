using System;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    class ToolkitAbbreviateHandler : IHttpHandler, IRequiresSessionState
    {
        private readonly string fAbbrHead;
        private readonly AbbreviateConfigItem fConfig;

        public ToolkitAbbreviateHandler(string abbrHead)
        {
            fAbbrHead = abbrHead;
            fConfig = WebAppSetting.WebCurrent.GetAbbreivateConfig(fAbbrHead);
        }

        #region IHttpHandler 成员

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            if (fConfig == null)
                response.Write(string.Format(ObjectUtil.SysCulture,
                    "无法翻译{0}，请检查Abbr.xml的配置是否存在该项目", fAbbrHead));
            else
            {
                string url = fConfig.Path;
                string queryString = context.Request.QueryString.ToString();
                if (!string.IsNullOrEmpty(queryString))
                {
                    url += string.Format(ObjectUtil.SysCulture, "{0}={1}", fConfig.IdName, queryString);
                }
                url = WebUtil.ResolveUrl(url);
                Uri uri = new Uri(context.Request.Url, url);
                WebResponse webResp = NetUtil.HttpGet(uri);
                string data = NetUtil.GetResponseData(webResp, Encoding.UTF8);
                response.Write(data);
            }
        }

        #endregion
    }
}
