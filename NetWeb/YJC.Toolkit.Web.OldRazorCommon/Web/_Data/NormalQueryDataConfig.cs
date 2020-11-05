using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [RazorDataConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2015-10-26", Author = "YJC",
        Description = "匹配NormalQueryCondition和NormalQueryResult模板使用的数据")]
    internal class NormalQueryDataConfig : BaseBootcssDataConfig
    {
        public override object CreateObject(params object[] args)
        {
            NormalQueryData result = new NormalQueryData(this);
            SetRazorField(result);
            return result;
        }

        [SimpleAttribute(DefaultValue = RazorDataConst.NO_DATA_CAPTION)]
        public string NoDataCaption { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool ShowTitle { get; set; }

        [SimpleAttribute]
        public bool ShrinkMeta { get; private set; }

        [SimpleAttribute]
        public bool HideCaption { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool Export { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool Sortable { get; private set; }

        [SimpleAttribute]
        public Alignment HeadAlign { get; private set; }

        [SimpleAttribute]
        public string ResponseFunc { get; private set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.QUERY_BUTTON_CAPTION)]
        public string ButtonCaption { get; private set; }
    }
}
