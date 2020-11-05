using System;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class BootcssDetailData : BaseBootcssData
    {
        public BootcssDetailData()
        {
            CaptionColumn = RazorDataConst.DETAIL_CAPTION_COLUMN;
            DataColumn = RazorDataConst.DETAIL_CONTROL_COLUMN;
            AppendHint = true;
            MainPicNickName = RazorDataConst.MAIN_PIC_NICKNAME;
            PictureTableIdName = RazorDataConst.PICTURE_TABLEID_NAME;
            TitleFormat = RazorDataConst.DETAIL_FORMAT;
        }

        internal BootcssDetailData(BootcssDetailDataConfig config)
        {
            ShowPicture = config.ShowPicture;
            CaptionColumn = config.CaptionColumn;
            DataColumn = config.DataColumn;
            AppendHint = config.AppendHint;
            PictureTable = config.PictureTable;
            PictureTableIdName = config.PictureTableIdName;
            MainPicNickName = config.MainPicNickName;
            TitleFormat = config.TitleFormat;
        }

        public bool ShowPicture { get; set; }

        public int CaptionColumn { get; set; }

        public int DataColumn { get; set; }

        public bool AppendHint { get; set; }

        public string PictureTable { get; set; }

        public string PictureTableIdName { get; set; }

        public string MainPicNickName { get; set; }

        public string TitleFormat { get; set; }
    }
}
