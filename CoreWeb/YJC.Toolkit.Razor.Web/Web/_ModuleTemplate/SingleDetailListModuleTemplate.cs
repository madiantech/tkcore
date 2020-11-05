using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Web
{
    [ModuleTemplate(Author = "YJC", CreateDate = "2017-05-03",
        Description = "定义单表增删改查，Detail为多表的模板")]
    internal class SingleDetailListModuleTemplate : BaseEditModuleTemplate
    {
        public SingleDetailListModuleTemplate()
            : base("Edit", "MultiDetail", "List")
        {
            AddPageTemplate(new PageTemplateInfo((source, input, output) =>
                MetaDataUtil.StartsWith(input.Style, "DetailList"), "DetailList"));
        }
    }
}