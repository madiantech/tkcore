namespace YJC.Toolkit.Sys
{
    public interface IRedirector
    {
        string Redirect(ISource source, IPageData pageData, OutputData data);
    }
}
