using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [ModuleTemplate(Author = "YJC", CreateDate = "2017-05-06",
        Description = "定义对话框模式下的树形单表增删改查的模板")]
    internal class SingleTreeDialogModuleTemplate : SingleDialogModuleTemplate
    {
        public override void SetPageData(ISource source, IInputData input,
            OutputData outputData, object pageData)
        {
            InternalRazorUtil.SetDialogMode(pageData, true);
        }
    }
}