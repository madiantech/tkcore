using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [MatchRuleConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-12-28", Description = "匹配弹出系统拍照发图的事件")]
    internal class EventPicSysPhotoRuleConfig : BaseEventKeyRuleConfig
    {
        public override RuleAttribute CreateObject(params object[] args)
        {
            return new PicSysPhotoEventAttribute(EventKey);
        }
    }
}
