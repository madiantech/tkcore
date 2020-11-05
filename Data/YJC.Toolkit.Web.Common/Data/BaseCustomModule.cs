using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseCustomModule : IModule
    {
        protected BaseCustomModule(string title)
        {
            TkDebug.AssertArgumentNullOrEmpty(title, "title", null);

            Title = title;
        }

        #region IModule 成员

        public string Title { get; private set; }

        public virtual IMetaData CreateMetaData(IPageData pageData)
        {
            return null;
        }

        public abstract ISource CreateSource(IPageData pageData);

        public virtual IPostObjectCreator CreatePostCreator(IPageData pageData)
        {
            return WebAppSetting.WebCurrent.DefaultPostCreator.CreateObject(pageData);
        }

        public abstract IPageMaker CreatePageMaker(IPageData pageData);

        public virtual IRedirector CreateRedirector(IPageData pageData)
        {
            return WebAppSetting.WebCurrent.DefaultRedirector.CreateObject(pageData);
        }

        public virtual bool IsSupportLogOn(IPageData pageData)
        {
            return true;
        }

        public virtual bool IsDisableInjectCheck(IPageData pageData)
        {
            return false;
        }

        public virtual bool IsCheckSubmit(IPageData pageData)
        {
            return false;
        }

        #endregion
    }
}
