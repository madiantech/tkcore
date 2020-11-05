using YJC.Toolkit.Data;
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
            AddPageMaker(new PageMakerInfo(InternalRazorUtil.IsStyle(PageStyle.List),
                   new JsonObjectPageMaker()));
            AddPageMaker(new PageMakerInfo(InternalRazorUtil.IsOperation(
                TreeOperationSource.MOVE_NODE, TreeOperationSource.MOVE_UP_DOWN),
                new PostPageMaker(ContentDataType.Json, PageStyle.Custom, new CustomUrlConfig())));
            AddPageMaker(new PageMakerInfo(InternalRazorUtil.IsEditStyle(true, PageStyle.Insert),
                   CreateEditPostPageMaker(PageStyle.Insert)));
            AddPageMaker(new PageMakerInfo(InternalRazorUtil.IsEditStyle(true, PageStyle.Update),
                   CreateEditPostPageMaker(PageStyle.Update)));
            AddPageMaker(new PageMakerInfo(InternalRazorUtil.IsStyle(PageStyle.Delete),
                new PostPageMaker(ContentDataType.Json, PageStyle.Custom,
                new CustomUrlConfig(false, false, "ListRefresh"))));
        }

        protected virtual PostPageMaker CreateEditPostPageMaker(PageStyle style)
        {
            return new PostPageMaker(ContentDataType.Json, PageStyle.List, null);
        }

        protected override void InitPageTemplate()
        {
            AddPageTemplate(new PageTemplateInfo(
                InternalRazorUtil.IsOperation("", null), TreeTemplate));
            AddPageTemplate(new PageTemplateInfo(
                InternalRazorUtil.IsStyle(PageStyle.Detail), DetailTemplate));
            AddPageTemplate(new PageTemplateInfo(
                InternalRazorUtil.IsEditStyle(false), EditTemplate));
        }
    }
}