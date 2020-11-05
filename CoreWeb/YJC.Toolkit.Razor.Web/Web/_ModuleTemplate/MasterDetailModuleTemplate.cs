using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Web
{
    [ModuleTemplate(Author = "YJC", CreateDate = "2017-05-06",
        Description = "定义主从表增删改查的模板")]
    internal class MasterDetailModuleTemplate : BaseEditModuleTemplate
    {
        public MasterDetailModuleTemplate()
            : base("MultiEdit", "MultiDetail", "List")
        {
            AddPageTemplate(new PageTemplateInfo((source, input, output) =>
               MetaDataUtil.StartsWith(input.Style, "DetailList"), "DetailList"));
        }
    }
}