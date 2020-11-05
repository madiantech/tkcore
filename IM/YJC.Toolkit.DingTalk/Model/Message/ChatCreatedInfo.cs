using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class ChatCreatedInfo : BaseChat
    {
        protected ChatCreatedInfo()
        {
        }

        public ChatCreatedInfo(string name, string owner, List<string> userIdList)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", null);
            TkDebug.AssertArgumentNullOrEmpty(owner, "owner", null);
            TkDebug.AssertArgumentNull(userIdList, "userIdList", null);

            Name = name;
            Owner = owner;
            UserIdList = userIdList;
        }

        [SimpleElement(NamingRule = NamingRule.Lower, IsMultiple = true)]//群成员列表
        public List<string> UserIdList { get; private set; }
    }
}