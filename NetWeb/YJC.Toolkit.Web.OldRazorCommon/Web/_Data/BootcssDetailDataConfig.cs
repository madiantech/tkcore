using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [RazorDataConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-06-25",
       Author = "YJC", Description = "匹配BootcssDetail模板使用的数据")]
    internal class BootcssDetailDataConfig : BaseBootcssDataConfig
    {
        [SimpleAttribute]
        public bool ShowPicture { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.DETAIL_CAPTION_COLUMN)]
        public int CaptionColumn { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.DETAIL_CONTROL_COLUMN)]
        public int DataColumn { get; protected set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool AppendHint { get; protected set; }

        [SimpleAttribute]
        public string PictureTable { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.PICTURE_TABLEID_NAME)]
        public string PictureTableIdName { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.MAIN_PIC_NICKNAME)]
        public string MainPicNickName { get; protected set; }

        [SimpleAttribute(DefaultValue = RazorDataConst.DETAIL_FORMAT)]
        public string TitleFormat { get; protected set; }

        public override object CreateObject(params object[] args)
        {
            BootcssDetailData result = new BootcssDetailData(this);
            SetRazorField(result);

            return result;
        }
    }
}