namespace YJC.Toolkit.Web
{
    [ModuleTemplate(Author = "YJC", CreateDate = "2020-04-13",
        Description = "定义基于对象的单表增删改查的模板")]
    internal class SingleObjectModuleTemplate : SingleModuleTemplate
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