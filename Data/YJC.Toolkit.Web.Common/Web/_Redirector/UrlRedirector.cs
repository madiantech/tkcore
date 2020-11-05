using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal sealed class UrlRedirector : IRedirector
    {
        /// <summary>
        /// Initializes a new instance of the UrlRedirector class.
        /// </summary>
        public UrlRedirector(string url)
        {
            Url = url;
        }

        public string Url { get; private set; }

        #region IRedirector 成员

        string IRedirector.Redirect(ISource source, IPageData pageData, OutputData data)
        {
            return Url;
        }

        #endregion
    }
}
