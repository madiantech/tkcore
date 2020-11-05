using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Web
{
    [ModuleTemplate(Author = "YJC", CreateDate = "2019-05-06",
        Description = "定义单表增删改查，Detail为多表的模板，DetailList为统计模型")]
    internal class SingleDetailStatListModuleTemplate : BaseEditModuleTemplate
    {
        public SingleDetailStatListModuleTemplate()
            : base("Edit", "MultiDetail", "List")
        {
            AddPageTemplate(new PageTemplateInfo((source, input, output) =>
                MetaDataUtil.StartsWith(input.Style, "DetailList"), "DetailStatList"));
        }
    }
}