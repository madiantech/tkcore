using YJC.Toolkit.Collections;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [RazorDataConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2019-05-05", Author = "YJC",
        Description = "匹配DbStatListSource对应模板使用的数据")]
    internal class NormalStatListDataConfig : NormalListDataConfig
    {
        [SimpleAttribute(DefaultValue = RazorDataConst.SUB_TOTAL_CAPTION)]
        public string SubTotalCaption { get; set; }

        [SimpleAttribute(DefaultValue = DataDirection.Foot)]
        public DataDirection SubTotalPosition { get; set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.TOTAL_CAPTION)]
        public string TotalCaption { get; set; }

        [SimpleAttribute(DefaultValue = DataDirection.Foot)]
        public DataDirection TotalPosition { get; set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "FieldDisplay")]
        public RegNameList<StatFieldDisplayConfig> FieldDisplayList { get; private set; }

        public override void OnReadObject()
        {
            if (OperatorPosition == null)
                OperatorPosition = YJC.Toolkit.Razor.OperatorPosition.None;
        }

        public override object CreateObject(params object[] args)
        {
            return new NormalStatListData(this);
        }
    }
}