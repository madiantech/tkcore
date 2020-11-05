using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ModuleTemplate(Author = "YJC", CreateDate = "2017-04-22",
        Description = "定义对话框模式下的单表增删改查的模板")]
    internal class SingleDialogModuleTemplate : SingleModuleTemplate
    {
        public override void SetPageData(ISource source, IInputData input,
            OutputData outputData, object pageData)
        {
            InternalRazorUtil.SetDialogMode(pageData, true);
        }

        protected override PostPageMaker CreateEditPostPageMaker()
        {
            return new PostPageMaker(ContentDataType.Json, PageStyle.Custom,
                new CustomUrlConfig(false, false, "CloseDialogAndRefresh"));
        }
    }
}