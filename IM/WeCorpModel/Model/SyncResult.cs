using System.Collections.Generic;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model
{
    //获取异步任务结果
    public class SyncResult : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Status { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Type { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Total { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public int PercentAge { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<ResultDetail> Result { get; set; }
    }
}