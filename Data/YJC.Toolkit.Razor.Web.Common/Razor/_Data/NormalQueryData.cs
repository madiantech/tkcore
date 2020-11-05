using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    public class NormalQueryData : BaseBootcssData
    {
        public NormalQueryData()
        {
            object defaultCreator = DefaultUtil.GetFactoryObject(RazorDataConst.SECTION_NAME,
                nameof(NormalQueryData));
            if (defaultCreator == null || !DefaultUtil.CreateConfigObject(
                defaultCreator, out object defaultObject))
            {
                ShowTitle = Export = Sortable = true;
                NoDataCaption = RazorDataConst.NO_DATA_CAPTION;
                HeadAlign = Alignment.Left;
                ButtonCaption = RazorDataConst.QUERY_BUTTON_CAPTION;
            }
            else
            {
                this.CopyFromObject(defaultObject);
            }
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

        [SimpleAttribute]
        public bool ShowTitle { get; set; }

        [SimpleAttribute]
        public bool HideCaption { get; set; }

        [SimpleAttribute]
        public bool ShrinkMeta { get; set; }

        [SimpleAttribute]
        public string NoDataCaption { get; set; }

        [SimpleAttribute]
        public bool Export { get; set; }

        [SimpleAttribute]
        public bool Sortable { get; set; }

        [SimpleAttribute]
        public Alignment HeadAlign { get; set; }

        [SimpleAttribute]
        public string ResponseFunc { get; set; }

        [SimpleAttribute]
        public string ButtonCaption { get; set; }
    }
}