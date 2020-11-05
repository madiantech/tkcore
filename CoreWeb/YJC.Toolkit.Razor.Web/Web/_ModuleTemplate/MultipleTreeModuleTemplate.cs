using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Web
{
    [ModuleTemplate(Author = "YJC", CreateDate = "2017-05-06",
        Description = "定义树形多表增删改查的模板")]
    internal class MultipleTreeModuleTemplate : BaseTreeModuleTemplate
    {
        public MultipleTreeModuleTemplate()
            : base("MultiEdit", "TreeMultiDetail", "Tree")
        {
            AddPageTemplate(new PageTemplateInfo((source, input, output) =>
                MetaDataUtil.StartsWith(input.Style, "DetailList"), "DetailList"));
        }

        protected override PostPageMaker CreateEditPostPageMaker(PageStyle style)
        {
            return new TreeEditPostPageMaker(style);
        }
    }
}