using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class AppMessageProgress : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int ProgressInPercent { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(EnumIntTypeConverter), UseObjectType = true)]
        public TaskStatus Status { get; set; }
    }
}