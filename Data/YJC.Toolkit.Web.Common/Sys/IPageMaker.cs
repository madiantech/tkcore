namespace YJC.Toolkit.Sys
{
    public interface IPageMaker
    {
        IContent WritePage(ISource source, IPageData pageData, OutputData outputData);
    }
}
