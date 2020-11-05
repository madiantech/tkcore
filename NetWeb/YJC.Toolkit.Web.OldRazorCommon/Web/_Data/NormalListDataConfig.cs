using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [RazorDataConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-06-30",
        Author = "YJC", Description = "匹配NormalList、NormalObjectList和NormalDetailList模板使用的数据")]
    internal class NormalListDataConfig : BaseBootcssDataConfig, IReadObjectCallBack
    {
        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public RazorOutputData RowDisplay { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public RazorOutputData RowOperator { get; protected set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool ShowPage { get; protected set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool ShowListHeader { get; protected set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool ShowTitle { get; set; }

        [SimpleAttribute]
        public TableDisplayType Display { get; set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.NO_DATA_CAPTION)]
        public string NoDataCaption { get; protected set; }

        [SimpleAttribute]
        public OperatorPosition? OperatorPosition { get; internal set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.OPERATION_CAPTION)]
        public string OperationCaption { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.NORMAL_LIST_PAGE_COUNT)]
        public int PageNumberCount { get; set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.NORMAL_LIST_OPERATOR_WIDTH)]
        public int OperatorWidth { get; protected set; }

        [SimpleAttribute]
        public TabDisplayType TabDisplayType { get; protected set; }

        [SimpleAttribute]
        public SearchDataMethod SearchCheckBox { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.NORMAL_DIALOG_HEIGHT)]
        public int DialogHeight { get; set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool ShowExportExcel { get; set; }

        [SimpleAttribute]
        public bool ShowHintInHead { get; set; }

        [SimpleAttribute]
        public bool ShowTabCount { get; set; }

        [ObjectElement(NamespaceType = NamespaceType.Toolkit)]
        public RowOperatorStyle RowOperatorStyle { get; protected set; }

        public override object CreateObject(params object[] args)
        {
            NormalListData result = new NormalListData(this);
            SetRazorField(result);

            return result;
        }

        #region IReadObjectCallBack 成员

        public virtual void OnReadObject()
        {
            if (OperatorPosition == null)
                OperatorPosition = YJC.Toolkit.Razor.OperatorPosition.Left;
        }

        #endregion IReadObjectCallBack 成员
    }
}