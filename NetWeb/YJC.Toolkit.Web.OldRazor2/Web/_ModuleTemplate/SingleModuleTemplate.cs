namespace YJC.Toolkit.Web
{
    [ModuleTemplate(Author = "YJC", CreateDate = "2017-04-21",
        Description = "定义单表增删改查的模板")]
    internal class SingleModuleTemplate : BaseEditModuleTemplate
    {
        public SingleModuleTemplate()
            : base("Edit", "Detail", "List")
        {
        }
    }
}