using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Company
{
    //标签管理（增加标签成员）
    internal class TagMemberParam
    {
        public TagMemberParam(int tagId, List<string> userList, List<int> partyList)
        {
            TkDebug.Assert(userList != null || partyList != null,
                $"{nameof(userList)}和{nameof(partyList)}不能同时为空", null);
            if (userList != null)
                TkDebug.AssertArgument(userList.Count < 1000, nameof(userList),
                    $"{nameof(userList)}长度不超过1000", null);
            if (partyList != null)
                TkDebug.AssertArgument(partyList.Count < 100, nameof(partyList),
                    $"{nameof(partyList)}长度不超过100", null);

            TagId = tagId;
            UserList = userList;
            PartyList = partyList;
        }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public int TagId { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Lower, IsMultiple = true)]
        public List<string> UserList { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Lower, IsMultiple = true)]
        public List<int> PartyList { get; private set; }
    }
}