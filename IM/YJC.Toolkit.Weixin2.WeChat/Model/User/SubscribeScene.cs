using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Model.User
{
    public enum SubscribeScene
    {
        [EnumFieldValue("ADD_SCENE_SEARCH")]
        Search,

        [EnumFieldValue("ADD_SCENE_ACCOUNT_MIGRATION")]
        AccountMigration,

        [EnumFieldValue("ADD_SCENE_PROFILE_CARD")]
        ProfileCard,

        [EnumFieldValue("ADD_SCENE_QR_CODE")]
        QRCode,

        [EnumFieldValue("ADD_SCENEPROFILE")]
        Profile,

        [EnumFieldValue("ADD_SCENE_PROFILE_ITEM ")]
        ProfileItem,

        [EnumFieldValue("ADD_SCENE_PAID")]
        Paid,

        [EnumFieldValue("ADD_SCENE_OTHERS")]
        Others
    }
}