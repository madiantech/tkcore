using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class ChatUpdatedInfo : BaseChat
    {
        public ChatUpdatedInfo(ChatInfo chat, List<string> addUserIdList, List<string> delUserIdList)
        {
            TkDebug.AssertArgumentNull(chat, "chat", null);

            ChatId = chat.ChatId;
            Name = chat.Name;
            Owner = chat.Owner;
            Icon = chat.Icon;
            ChatBannedType = chat.ChatBannedType;
            Searchable = chat.Searchable;
            ValidationType = chat.ValidationType;
            MentionAllAuthority = chat.MentionAllAuthority;
            ShowHistoryType = chat.ShowHistoryType;
            ManagementType = chat.ManagementType;
            AddUserIdList = addUserIdList;
            DelUserIdList = delUserIdList;
        }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string ChatId { get; private set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<string> AddUserIdList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<string> DelUserIdList { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Icon { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}