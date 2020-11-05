using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Company
{
    //成员管理（创建成员）
    public class SimpleUser : BaseResult, IEntity
    {
        public SimpleUser()
        {
        }

        [SimpleElement(LocalName = "userid", Order = 10)]
        // [NameModel(WeCorpConst.USER_MODE, LocalName = "UserLogonName")]
        public string Id { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 20)]
        //[NameModel(WeCorpConst.USER_MODE)]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        [SimpleElement(NamingRule = NamingRule.Camel, IsMultiple = true, Order = 50, UseSourceType = true)]
        public List<int> Department { get; set; }
    }
}