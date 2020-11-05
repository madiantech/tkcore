using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class BaseModuleTemplate : IModuleTemplate
    {
        private bool fInitPageMaker;
        private bool fInitPageTemplate;

        protected BaseModuleTemplate()
        {
            Templates = new List<PageTemplateInfo>();
            PageMakers = new List<PageMakerInfo>();
        }

        #region IModuleTemplate 成员

        public IEnumerable<PageTemplateInfo> PageTemplates
        {
            get
            {
                InternalInitPageTemplate();
                return Templates;
            }
        }

        public IEnumerable<PageMakerInfo> CreatePageMakers(IPageData pageData)
        {
            InternalInitPageMakers();
            return PageMakers;
        }

        public virtual void SetPageData(ISource source, IInputData input,
            OutputData outputData, object pageData)
        {
        }

        public virtual void SetPageMaker(ISource source, IInputData input,
            OutputData outputData, IPageMaker pageMaker)
        {
            if (input.IsPost && (input.Style.Style == PageStyle.Update))
            {
                PostPageMaker maker = pageMaker as PostPageMaker;
                if (maker != null)
                    maker.UseRetUrlFirst = true;
            }
        }

        #endregion IModuleTemplate 成员

        public List<PageTemplateInfo> Templates { get; private set; }

        public List<PageMakerInfo> PageMakers { get; private set; }

        private void InternalInitPageTemplate()
        {
            if (!fInitPageTemplate)
            {
                InitPageTemplate();
                fInitPageTemplate = true;
            }
        }

        private void InternalInitPageMakers()
        {
            if (!fInitPageMaker)
            {
                InitPageMakers();
                fInitPageMaker = true;
            }
        }

        protected abstract void InitPageMakers();

        protected abstract void InitPageTemplate();

        protected void AddPageTemplate(PageTemplateInfo pageTemplateInfo)
        {
            TkDebug.AssertArgumentNull(pageTemplateInfo, "pageTemplateInfo", this);

            Templates.Add(pageTemplateInfo);
        }

        protected void AddPageMaker(PageMakerInfo pageMakerInfo)
        {
            TkDebug.AssertArgumentNull(pageMakerInfo, "pageMakerInfo", this);

            PageMakers.Add(pageMakerInfo);
        }
    }
}