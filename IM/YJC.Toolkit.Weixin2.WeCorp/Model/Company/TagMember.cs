using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Company
{
    //标签管理（获取标签成员，删除标签成员）
    public class TagMember : Tag
    {
        [ObjectElement(NamingRule = NamingRule.Lower, IsMultiple = true)]
        public List<SimpleUser> UserList { get; private set; }

        [SimpleElement(Order = 40, IsMultiple = true, NamingRule = NamingRule.Lower)]
        public List<int> PartyList { get; private set; }
    }
}