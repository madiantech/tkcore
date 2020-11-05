using System;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    public class NormalListData : BaseBootcssData
    {
        public NormalListData()
        {
            object defaultCreator = DefaultUtil.GetFactoryObject(RazorDataConst.SECTION_NAME,
                nameof(NormalListData));
            if (defaultCreator == null || !DefaultUtil.CreateConfigObject(
                defaultCreator, out object defaultObject))
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
            else
            {
                this.CopyFromObject(defaultObject);
                if (RowOperatorStyle == null)
                    RowOperatorStyle = RowOperatorStyle.CreateDefault();
            }
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
            ShowMultiSelect = config.ShowMultiSelect;
        }

        [SimpleAttribute]
        public TableDisplayType Display { get; set; }

        [ObjectElement]
        public RazorOutputData RowDisplay { get; set; }

        [ObjectElement]
        public RazorOutputData RowOperator { get; set; }

        [SimpleAttribute]
        public bool ShowPage { get; set; }

        [SimpleAttribute]
        public bool ShowListHeader { get; set; }

        [SimpleAttribute]
        public string NoDataCaption { get; set; }

        [SimpleAttribute]
        public OperatorPosition OperatorPosition { get; set; }

        [SimpleAttribute]
        public string OperationCaption { get; set; }

        [SimpleAttribute]
        public int PageNumberCount { get; set; }

        [SimpleAttribute]
        public bool ShowMultiSelect { get; set; }

        [SimpleAttribute]
        public int OperatorWidth { get; set; }

        [SimpleAttribute]
        public TabDisplayType TabDisplayType { get; set; }

        [SimpleAttribute]
        public SearchDataMethod SearchCheckBox { get; set; }

        [SimpleAttribute]
        public int DialogHeight { get; set; }

        [SimpleAttribute]
        public bool ShowTitle { get; set; }

        [SimpleAttribute]
        public bool ShowExportExcel { get; set; }

        [ObjectElement]
        public RowOperatorStyle RowOperatorStyle { get; set; }

        [SimpleAttribute]
        public bool ShowHintInHead { get; set; }

        [SimpleAttribute]
        public bool ShowTabCount { get; set; }
    }
}