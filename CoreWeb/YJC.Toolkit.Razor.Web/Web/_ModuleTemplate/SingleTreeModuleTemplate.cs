using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Web
{
    [ModuleTemplate(Author = "YJC", CreateDate = "2017-05-05",
        Description = "定义树形单表增删改查的模板")]
    internal class SingleTreeModuleTemplate : BaseTreeModuleTemplate
    {
        public SingleTreeModuleTemplate()
            : base("Edit", "TreeDetail", "Tree")
        {
        }

        protected override PostPageMaker CreateEditPostPageMaker(PageStyle style)
        {
            return new TreeEditPostPageMaker(style);
        }
    }
}