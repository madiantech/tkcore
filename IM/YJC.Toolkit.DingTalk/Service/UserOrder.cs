using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Service
{
    public enum UserOrder
    {
        [EnumFieldValue("entry_asc")]
        EntryAsc,

        [EnumFieldValue("entry_desc")]
        EntryDesc,

        [EnumFieldValue("modify_asc")]
        ModifyAsc,

        [EnumFieldValue("modify_desc")]
        ModifyDesc,

        [EnumFieldValue("custom")]
        Custom
    }
}