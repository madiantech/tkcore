using System.Collections.Generic;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [RazorDataConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-06-25",
        Author = "YJC", Description = "匹配BootcssList使用的数据")]
    class BootcssListDataConfig : BaseBootcssDataConfig
    {
        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public RazorOutputData RowDisplay { get; private set; }

        [SimpleAttribute]
        public bool UseGetMoreButton { get; set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.GET_MORE_BUTTON_CAPTION)]
        public string GetMoreButtonCaption { get; set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.NO_DATA_CAPTION)]
        public string NoDataCaption { get; set; }

        [SimpleAttribute]
        public string QueryResolverName { get; set; }

        [SimpleAttribute]
        public string QueryFieldName { get; set; }

        [SimpleAttribute]
        public string QueryTitle { get; set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.MOBILE_LIST_PAGE_COUNT)]
        public int PageNumberCount { get; set; }

        [SimpleAttribute]
        public DataDirection Direction { get; set; }

        [SimpleAttribute]
        public bool ShowListHeader { get; set; }

        [SimpleAttribute]
        public bool ShowBorder { get; set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "ListField",
            UseConstructor = true)]
        public List<BootcssListFieldConfig> ListFields { get; private set; }

        public override object CreateObject(params object[] args)
        {
            BootcssListData result = new BootcssListData(this);
            SetRazorField(result);

            return result;
        }
    }
}
