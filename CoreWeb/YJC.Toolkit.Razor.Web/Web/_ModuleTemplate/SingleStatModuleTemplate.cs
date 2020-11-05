namespace YJC.Toolkit.Web
{
    [ModuleTemplate(Author = "YJC", CreateDate = "2019-04-29",
        Description = "定义单表(列表支持统计的)增删改查的模板")]
    internal class SingleStatModuleTemplate : BaseEditModuleTemplate
    {
        public SingleStatModuleTemplate()
            : base("Edit", "Detail", "StatList")
        {
        }
    }
}