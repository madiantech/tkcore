using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "主从表新建，修改，删除等综合的RazorPageMaker",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-09-13")]
    internal class MasterDetailRazorPageMakerConfig : SingleRazorPageMakerConfig
    {
        protected override RazorPageMaker CreateDetailPageMaker(OverrideItemConfig config,
            IPageData pageData)
        {
            return CreateRazorPageMaker("NormalMultiDetail", config, pageData);
        }

        protected override RazorPageMaker CreateEditPageMaker(OverrideItemConfig config,
            IPageData pageData)
        {
            return CreateRazorPageMaker("NormalMultiEdit", config, pageData);
        }

        protected override IPageMaker CreateCustomPageMaker(OverrideItemConfig config,
            IPageData pageData, PageStyleClass style)
        {
            if (MetaDataUtil.StartsWith(style, "DetailList"))
                return InternalWebRazorUtil.CreateDetailListPageMaker(config, pageData);

            return base.CreateCustomPageMaker(config, pageData, style);
        }
    }
}