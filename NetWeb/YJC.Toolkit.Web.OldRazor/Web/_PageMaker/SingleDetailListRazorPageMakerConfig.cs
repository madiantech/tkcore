using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "表新建，修改，删除，列表为单表模式。Detail为多表模式的PageMaker",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-10-30")]
    internal class SingleDetailListRazorPageMakerConfig : SingleRazorPageMakerConfig
    {
        public SingleDetailListRazorPageMakerConfig()
        {
            DetailTemplateName = "NormalMultiDetail";
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