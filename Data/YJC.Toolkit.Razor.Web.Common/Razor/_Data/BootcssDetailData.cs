using System;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    public class BootcssDetailData : BaseBootcssData
    {
        public BootcssDetailData()
        {
            object defaultCreator = DefaultUtil.GetFactoryObject(RazorDataConst.SECTION_NAME,
                nameof(BootcssDetailData));
            if (defaultCreator == null || !DefaultUtil.CreateConfigObject(
                defaultCreator, out object defaultObject))
            {
                CaptionColumn = RazorDataConst.DETAIL_CAPTION_COLUMN;
                DataColumn = RazorDataConst.DETAIL_CONTROL_COLUMN;
                AppendHint = true;
                MainPicNickName = RazorDataConst.MAIN_PIC_NICKNAME;
                PictureTableIdName = RazorDataConst.PICTURE_TABLEID_NAME;
                TitleFormat = RazorDataConst.DETAIL_FORMAT;
            }
            else
            {
                this.CopyFromObject(defaultObject);
            }
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

        [SimpleAttribute]
        public bool ShowPicture { get; set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.CAPTION_COLUMN)]
        public int CaptionColumn { get; set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.CONTROL_COLUMN)]
        public int DataColumn { get; set; }

        [SimpleAttribute]
        public bool AppendHint { get; set; }

        [SimpleAttribute]
        public string PictureTable { get; set; }

        [SimpleAttribute]
        public string PictureTableIdName { get; set; }

        [SimpleAttribute]
        public string MainPicNickName { get; set; }

        [SimpleAttribute]
        public string TitleFormat { get; set; }
    }
}