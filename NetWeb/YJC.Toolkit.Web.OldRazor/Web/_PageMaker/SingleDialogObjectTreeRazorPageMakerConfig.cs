using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "Object新建，修改，删除等综合的TreeRazorPageMaker，新建修改等操作为Dialog界面",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-11-26")]
    class SingleDialogObjectTreeRazorPageMakerConfig : SingleDialogTreeRazorPageMakerConfig
    {
        public SingleDialogObjectTreeRazorPageMakerConfig()
        {
            ListTemplateName = "NormalObjectList";
            DetailTemplateName = "NormalObjectTreeDetail";
            EditTemplateName = "NormalObjectEdit";
            TreeTemplateName = "NormalObjectTree";
        }
    }
}
