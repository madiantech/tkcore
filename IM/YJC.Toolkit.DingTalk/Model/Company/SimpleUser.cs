using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class SimpleUser : BaseResult
    {
        public SimpleUser()
        {
        }

        public SimpleUser(string userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}