namespace YJC.Toolkit.Web
{
    [ModuleTemplate(Author = "YJC", CreateDate = "2020-04-13",
        Description = "定义基于对象的对话框模式下的单表增删改查的模板")]
    internal class SingleObjectDialogModuleTemplate : SingleDialogModuleTemplate
    {
        protected override void SetModelCreator(PageTemplateInfo info)
        {
            switch (info.PageTemplate)
            {
                case "Edit":
                    info.ModelCreator = "ObjectContainerEdit";
                    break;

                case "Detail":
                    info.ModelCreator = "ObjectContainerDetail";
                    break;

                case "List":
                    info.ModelCreator = "ObjectContainerList";
                    break;
            }
        }
    }
}