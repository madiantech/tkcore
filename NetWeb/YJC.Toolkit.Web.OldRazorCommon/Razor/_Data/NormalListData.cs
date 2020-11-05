using System;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class NormalListData : BaseBootcssData
    {
        public NormalListData()
        {
            ShowPage = ShowListHeader = ShowTitle = ShowExportExcel = true;
            OperatorPosition = OperatorPosition.Left;
            NoDataCaption = RazorDataConst.NO_DATA_CAPTION;
            OperationCaption = RazorDataConst.OPERATION_CAPTION;
            OperatorWidth = RazorDataConst.NORMAL_LIST_OPERATOR_WIDTH;
            PageNumberCount = RazorDataConst.NORMAL_LIST_PAGE_COUNT;
            SearchCheckBox = SearchDataMethod.Like;
            RowOperatorStyle = RowOperatorStyle.CreateDefault();
        }

        internal NormalListData(NormalListDataConfig config)
        {
            ShowPage = config.ShowPage;
            ShowListHeader = config.ShowListHeader;
            OperatorPosition = config.OperatorPosition.Value;
            NoDataCaption = config.NoDataCaption;
            OperationCaption = config.OperationCaption;
            PageNumberCount = config.PageNumberCount;
            RowDisplay = config.RowDisplay;
            Display = config.Display;
            RowOperator = config.RowOperator;
            OperatorWidth = config.OperatorWidth;
            TabDisplayType = config.TabDisplayType;
            SearchCheckBox = config.SearchCheckBox;
            DialogHeight = config.DialogHeight;
            ShowTitle = config.ShowTitle;
            ShowExportExcel = config.ShowExportExcel;
            RowOperatorStyle = config.RowOperatorStyle;
            if (RowOperatorStyle == null)
                RowOperatorStyle = RowOperatorStyle.CreateDefault();
            ShowHintInHead = config.ShowHintInHead;
            ShowTabCount = config.ShowTabCount;
        }

        public TableDisplayType Display { get; set; }

        public RazorOutputData RowDisplay { get; set; }

        public RazorOutputData RowOperator { get; set; }

        public bool ShowPage { get; set; }

        public bool ShowListHeader { get; set; }

        public string NoDataCaption { get; set; }

        public OperatorPosition OperatorPosition { get; set; }

        public string OperationCaption { get; set; }

        public int PageNumberCount { get; set; }

        public int OperatorWidth { get; set; }

        public TabDisplayType TabDisplayType { get; set; }

        public SearchDataMethod SearchCheckBox { get; set; }

        public int DialogHeight { get; set; }

        public bool ShowTitle { get; set; }

        public bool ShowExportExcel { get; set; }

        public RowOperatorStyle RowOperatorStyle { get; set; }

        public bool ShowHintInHead { get; set; }

        public bool ShowTabCount { get; set; }
    }
}