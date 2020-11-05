using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class BaseTreeModuleTemplate : BaseModuleTemplate
    {
        protected BaseTreeModuleTemplate(string editTemplate, string detailTemplate,
            string treeTemplate)
        {
            TkDebug.AssertArgumentNullOrEmpty(editTemplate, "editTemplate", null);
            TkDebug.AssertArgumentNullOrEmpty(detailTemplate, "detailTemplate", null);
            TkDebug.AssertArgumentNullOrEmpty(treeTemplate, "treeTemplate", null);

            EditTemplate = editTemplate;
            DetailTemplate = detailTemplate;
            TreeTemplate = treeTemplate;
        }

        public string EditTemplate { get; private set; }

        public string TreeTemplate { get; private set; }

        public string DetailTemplate { get; private set; }

        protected override void InitPageMakers()
        {
            PageMakers.Add(new PageMakerInfo(InternalRazorUtil.IsStyle(PageStyle.List),
                   new JsonObjectPageMaker()));
            PageMakers.Add(new PageMakerInfo(InternalRazorUtil.IsEditStyle(true),
                   new PostPageMaker(ContentDataType.Json, PageStyle.List, null)));
            PageMakers.Add(new PageMakerInfo(InternalRazorUtil.IsStyle(PageStyle.Delete),
                new PostPageMaker(ContentDataType.Json, PageStyle.Custom,
                new CustomUrlConfig(false, false, "ListRefresh"))));
        }

        protected override void InitPageTemplate()
        {
            Templates.Add(new PageTemplateInfo(
                InternalRazorUtil.IsOperation("", null), TreeTemplate));
            Templates.Add(new PageTemplateInfo(
                InternalRazorUtil.IsStyle(PageStyle.Detail), DetailTemplate));
            Templates.Add(new PageTemplateInfo(
                InternalRazorUtil.IsEditStyle(false), EditTemplate));
        }
    }
}