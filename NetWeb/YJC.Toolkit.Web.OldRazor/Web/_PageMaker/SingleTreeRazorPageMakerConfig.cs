using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "单表新建，修改，删除等综合的TreeRazorPageMaker，新建修改等操作为普通界面",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-09-21")]
    internal class SingleTreeRazorPageMakerConfig : BaseTreeRazorPageMakerConfig
    {
        protected override IPageMaker CreatePostEditPageMaker(OverrideItemConfig config, PageStyle style)
        {
            return new TreePostPageMaker(ContentDataType.Json, PageStyle.List, null);
        }

        protected override RazorPageMaker CreateEditPageMaker(OverrideItemConfig config, IPageData pageData)
        {
            MasterDetailTreeRazorPageMakerConfig.SetDefaultConfig(ref config, pageData);

            return base.CreateEditPageMaker(config, pageData);
        }
    }
}
