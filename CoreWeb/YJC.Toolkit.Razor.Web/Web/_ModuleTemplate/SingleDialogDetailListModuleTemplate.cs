using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ModuleTemplate(Author = "YJC", CreateDate = "2019-10-12",
        Description = "定义单表增删改查，Detail为多表的模板，新增修改为Dialog模式")]
    internal class SingleDialogDetailListModuleTemplate : SingleDetailListModuleTemplate
    {
        public override void SetPageData(ISource source, IInputData input, OutputData outputData, object pageData)
        {
            PageStyle style = input.Style.Style;
            if (style == PageStyle.Insert || style == PageStyle.Update)
                InternalRazorUtil.SetDialogMode(pageData, true);
        }

        protected override PostPageMaker CreateEditPostPageMaker()
        {
            return new PostPageMaker(ContentDataType.Json, PageStyle.Custom,
                new CustomUrlConfig(false, false, "CloseDialogAndRefresh"));
        }
    }
}