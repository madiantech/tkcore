using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class UrlRedirectorAttribute : BaseRedirectorAttrbiute
    {
        /// <summary>
        /// Initializes a new instance of the UrlRedirectorAttribute class.
        /// </summary>
        public UrlRedirectorAttribute(string url)
        {
            Url = url;
        }

        public string Url { get; private set; }

        public override IRedirector CreateRedirector(IPageData pageData)
        {
            return new UrlRedirector(Url);
        }
    }
}
