using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class NormalQueryData : BaseBootcssData
    {
        public NormalQueryData()
        {
            ShowTitle = Export = Sortable = true;
            NoDataCaption = RazorDataConst.NO_DATA_CAPTION;
            HeadAlign = Alignment.Left;
            ButtonCaption = RazorDataConst.QUERY_BUTTON_CAPTION;
        }

        internal NormalQueryData(NormalQueryDataConfig config)
        {
            ShowTitle = config.ShowTitle;
            ShrinkMeta = config.ShrinkMeta;
            HideCaption = config.HideCaption;
            NoDataCaption = config.NoDataCaption;
            Export = config.Export;
            HeadAlign = config.HeadAlign;
            ResponseFunc = config.ResponseFunc;
            Sortable = config.Sortable;
            ButtonCaption = config.ButtonCaption;
        }

        public bool ShowTitle { get; set; }

        public bool HideCaption { get; set; }

        public bool ShrinkMeta { get; set; }

        public string NoDataCaption { get; set; }

        public bool Export { get; set; }

        public bool Sortable { get; set; }

        public Alignment HeadAlign { get; set; }

        public string ResponseFunc { get; set; }

        public string ButtonCaption { get; set; }
    }
}
