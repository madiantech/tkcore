using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [MatchRuleConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-12-28", Description = "匹配弹出拍照或者相册发图的事件")]
    internal class EventPicPhotoAlbumRuleConfig : BaseEventKeyRuleConfig
    {
        public override RuleAttribute CreateObject(params object[] args)
        {
            return new PicPhotoAlbumEventAttribute(EventKey);
        }
    }
}
