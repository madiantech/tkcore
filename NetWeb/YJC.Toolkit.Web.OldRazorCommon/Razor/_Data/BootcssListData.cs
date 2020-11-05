using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class BootcssListData : BaseBootcssData
    {
        public BootcssListData()
        {
            NoDataCaption = RazorDataConst.NO_DATA_CAPTION;
            GetMoreButtonCaption = RazorDataConst.GET_MORE_BUTTON_CAPTION;
            ListFields = new List<BootcssListFieldConfig>();
            PageNumberCount = RazorDataConst.MOBILE_LIST_PAGE_COUNT;
            ShowBorder = true;
        }

        internal BootcssListData(BootcssListDataConfig config)
        {
            RowDisplay = config.RowDisplay;
            UseGetMoreButton = config.UseGetMoreButton;
            GetMoreButtonCaption = config.GetMoreButtonCaption;
            NoDataCaption = config.NoDataCaption;
            QueryFieldName = config.QueryFieldName;
            QueryResolverName = config.QueryResolverName;
            QueryTitle = config.QueryTitle;
            PageNumberCount = config.PageNumberCount;
            Direction = config.Direction;
            ShowListHeader = config.ShowListHeader;
            ListFields = config.ListFields;
            ShowBorder = config.ShowBorder;
            if (ListFields == null)
                ListFields = new List<BootcssListFieldConfig>();
        }

        public RazorOutputData RowDisplay { get; set; }

        public bool UseGetMoreButton { get; set; }

        public string GetMoreButtonCaption { get; set; }

        public string NoDataCaption { get; set; }

        public string QueryResolverName { get; set; }

        public string QueryFieldName { get; set; }

        public string QueryTitle { get; set; }

        public int PageNumberCount { get; set; }

        public DataDirection Direction { get; set; }

        public bool ShowListHeader { get; set; }

        public bool ShowBorder { get; set; }

        public List<BootcssListFieldConfig> ListFields { get; private set; }

        private void SetDisplayRow(RazorContentType contentType, string content)
        {
            RowDisplay = new RazorOutputData(contentType, content);
        }

        public void AddListField(string nickName, int col)
        {
            AddListField(nickName, col, null);
        }

        public void AddListField(string nickName, int col, string @class)
        {
            TkDebug.AssertArgumentNullOrEmpty(nickName, "nickName", this);
            TkDebug.AssertArgument(col > 0 && col <= 12, "col", string.Format(ObjectUtil.SysCulture,
                "col必须在1到12之间，当前值是{0}越界了", col), this);

            ListFields.Add(new BootcssListFieldConfig(nickName, col, @class));
        }

        public void SetRowDisplaySection(string sectionName)
        {
            TkDebug.AssertArgumentNullOrEmpty(sectionName, "sectionName", this);

            SetDisplayRow(RazorContentType.Section, sectionName);
        }

        public void SetRowDisplayText(string text)
        {
            TkDebug.AssertArgumentNull(text, "text", this);

            SetDisplayRow(RazorContentType.Text, text);
        }

        public void SetRowDisplayFile(string localFile)
        {
            TkDebug.AssertArgumentNull(localFile, "localFile", this);

            SetDisplayRow(RazorContentType.RazorFile, localFile);
        }
    }
}
