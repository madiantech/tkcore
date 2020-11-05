using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal sealed class RetUrlRedirector : IRedirector
    {
        public readonly static IRedirector Redirector = new RetUrlRedirector();

        private RetUrlRedirector()
        {
        }

        #region IRedirector 成员

        public string Redirect(ISource source, IPageData pageData, OutputData data)
        {
            string retUrl = pageData.QueryString["RetUrl"];
            if (string.IsNullOrEmpty(retUrl))
            {
                TkDebug.ThrowIfNoAppSetting();
                retUrl = WebAppSetting.WebCurrent.StartupPath;
            }
            return retUrl;
        }

        #endregion
    }
}
