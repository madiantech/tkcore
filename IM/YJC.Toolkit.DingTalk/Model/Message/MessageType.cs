using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public enum MessageType
    {
        [EnumFieldValue("text")]
        Text,

        [EnumFieldValue("image")]
        Image,

        [EnumFieldValue("voice")]
        Voice,

        [EnumFieldValue("file")]
        File,

        [EnumFieldValue("link")]
        Link,

        [EnumFieldValue("oa")]
        OA,

        [EnumFieldValue("markdown")]
        Markdown,

        [EnumFieldValue("action_card")]
        ActionCard
    }
}