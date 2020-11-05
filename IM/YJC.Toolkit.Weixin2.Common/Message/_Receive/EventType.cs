using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin.Message
{
    public enum EventType
    {
        [EnumFieldValue("")]
        None,

        [EnumFieldValue("subscribe")]
        Subscribe,

        [EnumFieldValue("unsubscribe")]
        Unsubscribe,

        [EnumFieldValue("SCAN")]
        Scan,

        [EnumFieldValue("LOCATION")]
        Location,

        [EnumFieldValue("CLICK")]
        Click,

        [EnumFieldValue("VIEW")]
        View,

        [EnumFieldValue("scancode_push")]
        ScanCodePush,

        [EnumFieldValue("scancode_waitmsg")]
        ScanCodeWaitmsg,

        [EnumFieldValue("pic_sysphoto")]
        PicSysPhoto,

        [EnumFieldValue("pic_photo_or_album")]
        PicPhotoAlbum,

        [EnumFieldValue("pic_weixin")]
        PicWeixin,

        [EnumFieldValue("MASSSENDJOBFINISH")]
        MassEndJobFinish,

        [EnumFieldValue("TEMPLATESENDJOBFINISH")]
        TemplateEndJobFinish,

        [EnumFieldValue("enter_agent")]
        EnterAgent,

        [EnumFieldValue("location_select")]
        LocationSelect
    }
}