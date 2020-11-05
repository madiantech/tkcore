using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class OutputRedirector : IRedirector
    {
        public readonly static IRedirector Redirector = new OutputRedirector();

        private OutputRedirector()
        {
        }

        #region IRedirector 成员

        string IRedirector.Redirect(ISource source, IPageData pageData, OutputData data)
        {
            TkDebug.AssertArgumentNull(data, "data", this);
            PageMakerUtil.AssertType(this, data, SourceOutputType.String);

            return data.Data.Convert<string>();
        }

        #endregion IRedirector 成员
    }
}