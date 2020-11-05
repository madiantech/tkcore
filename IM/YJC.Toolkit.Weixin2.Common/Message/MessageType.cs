using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin.Message
{
    public enum MessageType
    {
        [EnumFieldValue("text")]
        Text,

        [EnumFieldValue("image")]
        Image,

        [EnumFieldValue("voice")]
        Voice,

        [EnumFieldValue("video")]
        Video,

        Location,
        Link,
        Music,

        [EnumFieldValue("news")]
        News,

        [EnumFieldValue("file")]
        File,

        [EnumFieldValue("mpnews")]
        MpNews,

        MpVideo,
        TransferCustomerService,

        [EnumFieldValue("textcard")]
        TextCard,

        [EnumFieldValue("markdown")]
        Markdown,

        [EnumFieldValue("miniprogram_notice")]
        MiniprogramNotice,

        [EnumFieldValue("event")]
        Event
    }
}