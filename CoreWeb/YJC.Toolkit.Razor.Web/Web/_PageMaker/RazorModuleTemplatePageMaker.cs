using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class RazorModuleTemplatePageMaker : CompositePageMaker
    {
        private readonly IModuleTemplate fModuleTemplate;
        private readonly List<ModuleOverridePageTemplateConfigItem> fOverridePageTemplates;
        private readonly List<ModuleOverridePostPageMakerConfigItem> fOverridePostPageMakers;

        public RazorModuleTemplatePageMaker(string moduleTemplate, IPageData pageData)
            : base(pageData)
        {
            TkDebug.AssertArgumentNullOrEmpty(moduleTemplate, "moduleTemplate", null);

            fModuleTemplate = PlugInFactoryManager.CreateInstance<IModuleTemplate>(
                ModuleTemplatePlugInFactory.REG_NAME, moduleTemplate);
            if (fModuleTemplate.PageTemplates != null)
            {
                foreach (var item in fModuleTemplate.PageTemplates)
                {
                    RazorPageTemplatePageMaker pageMaker = new RazorPageTemplatePageMaker(item.PageTemplate, pageData)
                    {
                        ModelCreator = item.ModelCreator
                    };
                    Add(item.Function, pageMaker);
                    pageMaker.SetPageData += pageMaker_SetPageData;
                }
            }
            var pageMakers = fModuleTemplate.CreatePageMakers(pageData);
            if (pageMakers != null)
            {
                foreach (var item in pageMakers)
                {
                    if (item.PageMaker != null)
                        Add(item.Function, item.PageMaker);
                }
            }

            SetCallInfo(pageData);
        }

        internal RazorModuleTemplatePageMaker(RazorModuleTemplatePageMakerConfig config, IPageData pageData)
            : this(config.ModuleTemplate, pageData)
        {
            fOverridePageTemplates = config.PageTemplates;
            fOverridePostPageMakers = config.PostPageMakers;

            var overrideItem = FindOverrideItem(pageData);
            if (overrideItem != null)
            {
                if (CallerInfo is RazorPageTemplatePageMaker pageMaker)
                {
                    pageMaker.ResetCallInfo(overrideItem.RetUrl, pageData);
                }
            }
        }

        private void pageMaker_SetPageData(object sender, PageDataEventArgs e)
        {
            fModuleTemplate.SetPageData(e.Source, e.Input, e.OutputData, e.PageData);
        }

        protected override void PrepareWritePage(IPageMaker pageMaker, ISource source,
            IPageData pageData, OutputData outputData)
        {
            base.PrepareWritePage(pageMaker, source, pageData, outputData);

            fModuleTemplate.SetPageMaker(source, pageData, outputData, pageMaker);
            RazorPageTemplatePageMaker templateMaker = pageMaker as RazorPageTemplatePageMaker;
            if (templateMaker != null)
            {
                var overrideItem = FindOverrideItem(pageData);
                templateMaker.SetConfig(overrideItem);
                return;
            }

            PostPageMaker postMaker = pageMaker as PostPageMaker;
            if (postMaker != null)
            {
                if (fOverridePostPageMakers == null)
                    return;
                var overrideItem = (from item in fOverridePostPageMakers
                                    where MetaDataUtil.Equals(item.Style, pageData.Style)
                                    select item).FirstOrDefault();
                if (overrideItem != null)
                {
                    postMaker.SetProperties(overrideItem.DataType,
                        overrideItem.DestUrl, overrideItem.CustomUrl);
                    postMaker.UseRetUrlFirst = overrideItem.UseRetUrlFirst;
                    if (overrideItem.AlertMessage != null)
                        postMaker.AlertMessage = overrideItem.AlertMessage.ToString();
                }
            }
        }

        private ModuleOverridePageTemplateConfigItem FindOverrideItem(IPageData pageData)
        {
            if (fOverridePageTemplates == null)
                return null;
            return (from item in fOverridePageTemplates
                    where MetaDataUtil.Equals(item.Style, pageData.Style)
                    select item).FirstOrDefault();
        }
    }
}