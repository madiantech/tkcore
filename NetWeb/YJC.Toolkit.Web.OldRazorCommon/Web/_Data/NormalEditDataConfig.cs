using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [RazorDataConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-07-03",
        Author = "YJC", Description = "匹配NormalEdit、NormalObjectEdit和NormalMultiEdit模板使用的数据")]
    internal class NormalEditDataConfig : BootcssEditDataConfig
    {
        [SimpleAttribute(DefaultValue = true)]
        public bool ShowTitle { get; set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.NORMAL_CANCEL_CAPTION)]
        public string NormalCancelCaption { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.DIALOG_CANCEL_CAPTION)]
        public string DialogCancelCaption { get; protected set; }

        [SimpleAttribute(DefaultValue = PageContainer.Fluid)]
        public PageContainer ContainerType { get; set; }

        public override object CreateObject(params object[] args)
        {
            NormalEditData result = new NormalEditData(this);
            SetRazorField(result);
            result.Initialize();

            return result;
        }
    }
}