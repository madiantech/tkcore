using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "表列表，删除为单表模式；新建，修改为Dialog界面；Detail为多表模式的PageMaker",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-10-30")]
    internal class SingleDialogDetailListRazorPageMakerConfig : SingleDialogRazorPageMakerConfig
    {
        public SingleDialogDetailListRazorPageMakerConfig()
        {
            DetailTemplateName = "NormalMultiDetail";
        }

        protected override IPageMaker CreateCustomPageMaker(OverrideItemConfig config,
            IPageData pageData, PageStyleClass style)
        {
            if (MetaDataUtil.StartsWith(style, "DetailList"))
            {
                return InternalWebRazorUtil.CreateDetailListPageMaker(config, pageData);
            }
            return base.CreateCustomPageMaker(config, pageData, style);
        }
    }
}