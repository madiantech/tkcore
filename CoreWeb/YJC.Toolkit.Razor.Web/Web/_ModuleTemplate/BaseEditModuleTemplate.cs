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
            AddPageTemplate(new PageTemplateInfo(
                InternalRazorUtil.IsStyle(PageStyle.List), ListTemplate));
            AddPageTemplate(new PageTemplateInfo(
                InternalRazorUtil.IsStyle(PageStyle.Detail), DetailTemplate));
            AddPageTemplate(new PageTemplateInfo(
                InternalRazorUtil.IsEditStyle(false), EditTemplate));
        }

        protected override void InitPageMakers()
        {
            AddPageMaker(new PageMakerInfo(InternalRazorUtil.IsEditStyle(true),
                CreateEditPostPageMaker()));
            PageMakers.Add(new PageMakerInfo(InternalRazorUtil.IsOperation(DbListSource.TAB_STYLE_OPERATION),
                new JsonObjectPageMaker()));
            AddPageMaker(new PageMakerInfo(InternalRazorUtil.IsStyle(PageStyle.Delete),
                CreateDeletePostPageMaker()));
        }

        protected virtual PostPageMaker CreateDeletePostPageMaker()
        {
            return new PostPageMaker(ContentDataType.Json, PageStyle.Custom,
                new CustomUrlConfig(false, false, "ListRefresh"));
        }

        protected virtual PostPageMaker CreateEditPostPageMaker()
        {
            return new PostPageMaker(ContentDataType.Json, PageStyle.List, null);
        }
    }
}