using System;
using System.Web;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public abstract class SimpleLogOnRight : EmptyLogOnRight
    {
        protected SimpleLogOnRight()
        {
            LogOnPage = WebAppSetting.WebCurrent.LogOnPath;
        }

        public string LogOnPage { get; set; }

        public override void CheckLogOn(object userId, Uri url)
        {
            string totalURL = LogOnPage;
            if (string.IsNullOrEmpty(totalURL))
                return;
            if (!WebGlobalVariable.Info.IsLogOn)
            {
                string urlStr = url.ToString();
                if (!string.IsNullOrEmpty(urlStr))
                {
                    string conj = (totalURL.IndexOf("?", StringComparison.Ordinal) == -1) ? "?" : "&";
                    totalURL = string.Format(ObjectUtil.SysCulture, "{2}{0}RetURL={1}", conj,
                        HttpUtility.UrlEncode(urlStr), totalURL);
                }
                throw new RedirectException(totalURL);
            }
        }

        public override void Initialize(IUserInfo user)
        {
        }
    }
}
