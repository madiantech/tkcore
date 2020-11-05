using System.Collections.Generic;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [RazorDataConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-06-25",
        Author = "YJC", Description = "匹配BootcssEdit和BootcssObjectEdit模板使用的数据")]
    internal class BootcssEditDataConfig : BaseBootcssDataConfig
    {
        [SimpleAttribute(DefaultValue = RazorDataConst.CAPTION_COLUMN)]
        public int CaptionColumn { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.CONTROL_COLUMN)]
        public int DataColumn { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.SAVE_BUTTON_CAPTION)]
        public string SaveButtonCaption { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.FORM_ACTION)]
        public string FormAction { get; protected set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool ShowCaption { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.EDIT_FORMAT)]
        public string EditFormat { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.INSERT_FORMAT)]
        public string InsertFormat { get; protected set; }

        [SimpleAttribute]
        public bool DialogMode { get; set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "GroupSection")]
        [TagElement(NamespaceType.Toolkit, LocalName = "ControlGroup")]
        public List<GroupSection> ControlGroupList { get; protected set; }

        public override object CreateObject(params object[] args)
        {
            BootcssEditData result = new BootcssEditData(this);
            SetRazorField(result);
            result.Initialize();

            return result;
        }
    }
}