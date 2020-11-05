using System.Collections.Generic;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [RazorDataConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-07-03", Author = "YJC",
        Description = "匹配NormalDetail、NormalObjectDetail、MultiTreeDetail、NormalMultiDetail、NormalTreeDetail和NormalObjectTreeDetail模板使用的数据")]
    internal class NormalDetailDataConfig : BootcssDetailDataConfig
    {
        [SimpleAttribute]
        public bool DialogMode { get; protected set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool IgnoreEmptyField { get; protected set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool ShowTitle { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.NORMAL_CANCEL_CAPTION)]
        public string NormalCancelCaption { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.DIALOG_CANCEL_CAPTION)]
        public string DialogCancelCaption { get; protected set; }

        [SimpleAttribute]
        public int DialogHeight { get; set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "GroupSection")]
        [TagElement(NamespaceType.Toolkit, LocalName = "ControlGroup")]
        public List<GroupSection> ControlGroupList { get; protected set; }

        public override object CreateObject(params object[] args)
        {
            NormalDetailData result = new NormalDetailData(this);
            SetRazorField(result);
            result.Initialize();

            return result;
        }
    }
}