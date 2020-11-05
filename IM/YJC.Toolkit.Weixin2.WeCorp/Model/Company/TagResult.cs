using System.Collections.Generic;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Company
{
    //添加标签成员返回参数
    public class TagResult : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Lower)]
        [TkTypeConverter(typeof(StringListTypeConverter))]
        public string InvalidList { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Lower, IsMultiple = true)]
        public List<int> InvalidParty { get; private set; }
    }
}