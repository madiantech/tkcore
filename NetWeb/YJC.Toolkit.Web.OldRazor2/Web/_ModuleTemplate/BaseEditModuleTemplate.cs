using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class BaseEditModuleTemplate : BaseModuleTemplate
    {
        protected BaseEditModuleTemplate(string editTemplate, string detailTemplate,
            string listTemplate)
        {
            TkDebug.AssertArgumentNullOrEmpty(editTemplate, "editTemplate", null);
            TkDebug.AssertArgumentNullOrEmpty(detailTemplate, "detailTemplate", null);
            TkDebug.AssertArgumentNullOrEmpty(listTemplate, "listTemplate", null);

            EditTemplate = editTemplate;
            DetailTemplate = detailTemplate;
            ListTemplate = listTemplate;
        }

        public string EditTemplate { get; private set; }

        public string ListTemplate { get; private set; }

        public string DetailTemplate { get; private set; }

        protected override void InitPageTemplate()
        {
            Templates.Add(new PageTemplateInfo(
                InternalRazorUtil.IsStyle(PageStyle.List), ListTemplate));
            Templates.Add(new PageTemplateInfo(
                InternalRazorUtil.IsStyle(PageStyle.Detail), DetailTemplate));
            Templates.Add(new PageTemplateInfo(
                InternalRazorUtil.IsEditStyle(false), EditTemplate));
        }

        protected override void InitPageMakers()
        {
            PageMakers.Add(new PageMakerInfo(InternalRazorUtil.IsEditStyle(true),
                   new PostPageMaker(ContentDataType.Json, PageStyle.List, null)));
            PageMakers.Add(new PageMakerInfo(InternalRazorUtil.IsOperation(DbListSource.TAB_STYLE_OPERATION),
                new JsonObjectPageMaker()));
            PageMakers.Add(new PageMakerInfo(InternalRazorUtil.IsStyle(PageStyle.Delete),
                new PostPageMaker(ContentDataType.Json, PageStyle.Custom,
                new CustomUrlConfig(false, false, "ListRefresh"))));
        }
    }
}