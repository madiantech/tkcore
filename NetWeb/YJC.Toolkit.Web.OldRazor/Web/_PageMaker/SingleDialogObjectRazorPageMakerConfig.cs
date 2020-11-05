using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "Object新建，修改，删除等综合的RazorPageMaker，新建修改等操作为Dialog界面",
       NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-08-06")]
    class SingleDialogObjectRazorPageMakerConfig : SingleDialogRazorPageMakerConfig
    {
        public SingleDialogObjectRazorPageMakerConfig()
        {
            ListTemplateName = "NormalObjectList";
            DetailTemplateName = "NormalObjectDetail";
            EditTemplateName = "NormalObjectEdit";
        }
    }
}
