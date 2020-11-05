using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "单表新建，修改，删除等综合的RazorPageMaker",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-11-20")]
    class SingleRazorPageMakerConfig : BaseSingleRazorPageMakerConfig
    {
        protected override IPageMaker CreatePostEditPageMaker(OverrideItemConfig config, PageStyle style)
        {
            PostPageMaker result = CreatePostPageMaker(PageStyle.List, null, config);
            bool useFirst;
            if (config != null)
                useFirst = config.UseRetUrlFirst;
            else
                useFirst = style == PageStyle.Update;
            result.UseRetUrlFirst = useFirst;
            return result;
        }
    }
}
