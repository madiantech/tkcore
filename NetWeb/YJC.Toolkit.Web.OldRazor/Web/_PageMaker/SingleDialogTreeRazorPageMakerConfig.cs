using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "单表新建，修改，删除等综合的TreeRazorPageMaker，新建修改等操作为Dialog界面",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-08-06")]
    class SingleDialogTreeRazorPageMakerConfig : BaseTreeRazorPageMakerConfig
    {
        protected override IPageMaker CreatePostEditPageMaker(OverrideItemConfig config, PageStyle style)
        {
            CustomUrlConfig defaultUrl = new CustomUrlConfig(false, false, "CloseDialogAndRefresh");
            return CreatePostPageMaker(PageStyle.Custom, defaultUrl, config);
        }

        protected override RazorPageMaker CreateEditPageMaker(OverrideItemConfig config, IPageData pageData)
        {
            RazorPageMaker pageMaker = base.CreateEditPageMaker(config, pageData);
            if (pageMaker.RazorData == null)
            {
                pageMaker.RazorData = WebRazorUtil.CreateDefaultDialogData();
            }
            else
            {
                NormalEditDataConfig data = pageMaker.RazorData as NormalEditDataConfig;
                if (data != null)
                {
                    data.ShowTitle = false;
                    data.DialogMode = true;
                }
            }
            return pageMaker;
        }
    }
}
