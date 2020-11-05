using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public interface IModule
    {
        string Title { get; }

        IMetaData CreateMetaData(IPageData pageData);

        ISource CreateSource(IPageData pageData);

        IPostObjectCreator CreatePostCreator(IPageData pageData);

        IPageMaker CreatePageMaker(IPageData pageData);

        IRedirector CreateRedirector(IPageData pageData);

        bool IsSupportLogOn(IPageData pageData);

        bool IsDisableInjectCheck(IPageData pageData);

        bool IsCheckSubmit(IPageData pageData);
    }
}
