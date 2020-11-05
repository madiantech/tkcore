using System;
using System.IO;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.HtmlExtension
{
    internal class HtmlModule : IModule
    {
        private static readonly ISource Source = new EmptySource();
        private readonly string fSource;

        public HtmlModule(string source)
        {
            fSource = source;
        }

        private class EmptySource : ISource
        {
            #region ISource 成员

            public OutputData DoAction(IInputData input)
            {
                return OutputData.Create(string.Empty);
            }

            #endregion ISource 成员
        }

        #region IModule 成员

        public IMetaData CreateMetaData(IPageData pageData)
        {
            return null;
        }

        public IPageMaker CreatePageMaker(IPageData pageData)
        {
            string fileName = Path.GetFileName(fSource) + ".html";
            string dirName = Path.GetDirectoryName(fSource);
            string virtualPath;
            if (string.IsNullOrEmpty(dirName))
                virtualPath = "html";
            else
                virtualPath = Path.Combine("html", dirName);
            return new HtmlFilePageMaker(virtualPath, fileName);
        }

        public IPostObjectCreator CreatePostCreator(IPageData pageData)
        {
            throw new NotSupportedException();
        }

        public IRedirector CreateRedirector(IPageData pageData)
        {
            throw new NotSupportedException();
        }

        public ISource CreateSource(IPageData pageData)
        {
            return Source;
        }

        public bool IsCheckSubmit(IPageData pageData)
        {
            return false;
        }

        public bool IsDisableInjectCheck(IPageData pageData)
        {
            return false;
        }

        public bool IsSupportLogOn(IPageData pageData)
        {
            return true;
        }

        public string Title
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion IModule 成员
    }
}