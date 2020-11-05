using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class XmlModuleTemplate : IModuleTemplate
    {
        private readonly ModuleTemplateConfigItem fConfig;
        private List<PageTemplateInfo> fTemplates;
        private List<PageMakerInfo> fPageMakers;

        public XmlModuleTemplate(ModuleTemplateConfigItem config)
        {
            fConfig = config;
        }

        #region IModuleTemplate 成员

        public IEnumerable<PageTemplateInfo> PageTemplates
        {
            get
            {
                if (fTemplates == null)
                {
                    fTemplates = new List<PageTemplateInfo>();
                    if (fConfig.PageTemplates != null)
                        foreach (var item in fConfig.PageTemplates)
                        {
                            PageTemplateInfo info = new PageTemplateInfo(item.Condition.UseCondition, item.Template)
                            {
                                ModelCreator = item.ModelCreator
                            };
                            fTemplates.Add(info);
                        }
                }
                return fTemplates;
            }
        }

        public IEnumerable<PageMakerInfo> CreatePageMakers(IPageData pageData)
        {
            if (fPageMakers != null)
            {
                fPageMakers = new List<PageMakerInfo>();
                if (fConfig.PageMakers != null)
                    foreach (var item in fConfig.PageMakers)
                    {
                        IPageMaker pageMaker = item.PageMaker.CreateObject(pageData);
                        PageMakerInfo info = new PageMakerInfo(item.Condition.UseCondition, pageMaker);
                        fPageMakers.Add(info);
                    }
            }
            return fPageMakers;
        }

        public void SetPageData(ISource source, IInputData input, OutputData outputData, object pageData)
        {
            ISupportDialog dialog = pageData as ISupportDialog;
            if (dialog != null)
                switch (fConfig.Mode)
                {
                    case PageDataMode.Normal:
                        dialog.SetDialogMode(false);
                        break;

                    case PageDataMode.Dialog:
                        dialog.SetDialogMode(true);
                        break;

                    case PageDataMode.EditDialog:
                        dialog.SetDialogMode(input.Style.Style == PageStyle.Insert
                            || input.Style.Style == PageStyle.Update);
                        break;
                }
        }

        public void SetPageMaker(ISource source, IInputData input, OutputData outputData, IPageMaker pageMaker)
        {
            if (input.IsPost && (input.Style.Style == PageStyle.Update))
            {
                PostPageMaker maker = pageMaker as PostPageMaker;
                if (maker != null)
                    maker.UseRetUrlFirst = true;
            }
        }

        #endregion IModuleTemplate 成员
    }
}