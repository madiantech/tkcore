using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    public class Administrator : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(EnumIntTypeConverter), UseObjectType = true)]
        public SysLevel SysLevel { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }
    }
}