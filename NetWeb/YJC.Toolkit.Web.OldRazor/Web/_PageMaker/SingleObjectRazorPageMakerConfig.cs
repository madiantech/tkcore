using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "Object新建，修改，删除等综合的RazorPageMaker",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-11-20")]
    class SingleObjectRazorPageMakerConfig : SingleRazorPageMakerConfig
    {
        public SingleObjectRazorPageMakerConfig()
        {
            ListTemplateName = "NormalObjectList";
            DetailTemplateName = "NormalObjectDetail";
            EditTemplateName = "NormalObjectEdit";
        }
    }
}
