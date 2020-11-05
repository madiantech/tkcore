using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ModuleTemplate(Author = "YJC", CreateDate = "2017-05-06",
        Description = "定义对话框模式下的树形单表增删改查的模板")]
    internal class SingleTreeDialogModuleTemplate : SingleTreeModuleTemplate
    {
        public override void SetPageData(ISource source, IInputData input,
            OutputData outputData, object pageData)
        {
            var style = input.Style.Style;
            if (style == PageStyle.Update || style == PageStyle.Insert)
                InternalRazorUtil.SetDialogMode(pageData, true);
        }

        protected override PostPageMaker CreateEditPostPageMaker(PageStyle style)
        {
            var pageMaker = new PostPageMaker(ContentDataType.Json, PageStyle.Custom,
                new CustomUrlConfig(false, false, "CloseDialogAndRefresh"));
            return pageMaker;
        }
    }
}