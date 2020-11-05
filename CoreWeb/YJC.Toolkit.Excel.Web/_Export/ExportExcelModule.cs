using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class ExportExcelModule : IModule
    {
        private readonly Tk5ModuleXml fXml;

        public ExportExcelModule(Tk5ModuleXml xml)
        {
            fXml = xml;
        }
        #region IModule 成员

        public string Title
        {
            get
            {
                return fXml.Title;
            }
        }

        public IMetaData CreateMetaData(IPageData pageData)
        {
            return fXml.CreateMetaData(pageData);
        }

        public ISource CreateSource(IPageData pageData)
        {
            return IsTemplate(pageData) ? new EmptySource(true) : fXml.CreateSource(pageData);
        }

        public IPostObjectCreator CreatePostCreator(IPageData pageData)
        {
            return fXml.CreatePostCreator(pageData);
        }

        public IPageMaker CreatePageMaker(IPageData pageData)
        {
            if (IsTemplate(pageData))
                return new ExportExcelHeaderPageMaker();
            else
                return new ExportExcelPageMaker();
        }

        public IRedirector CreateRedirector(IPageData pageData)
        {
            return fXml.CreateRedirector(pageData);
        }

        public bool IsSupportLogOn(IPageData pageData)
        {
            return fXml.IsSupportLogOn(pageData);
        }

        public bool IsDisableInjectCheck(IPageData pageData)
        {
            return fXml.IsDisableInjectCheck(pageData);
        }

        public bool IsCheckSubmit(IPageData pageData)
        {
            return fXml.IsCheckSubmit(pageData);
        }

        #endregion

        private static bool IsTemplate(IPageData pageData)
        {
            return pageData.QueryString["Excel"] == "template";
        }

    }
}
