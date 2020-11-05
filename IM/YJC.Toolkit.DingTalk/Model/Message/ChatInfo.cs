using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    //创建会话的返回结果
    public class ChatInfo : ChatCreatedInfo
    {
        [SimpleElement(NamingRule = NamingRule.Lower)]//群会话的ID
        public string ChatId { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]//会话类型，2表示企业群
        public int ConversationTag { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]//群头像mediaId
        public string Icon { get; set; }
    }
}