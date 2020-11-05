using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Company
{
    //成员管理（创建成员）
    public class ExternalProfile
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ExternalCorpName { get; set; }

        [ObjectElement(LocalName = "external_attr", IsMultiple = true)]
        public List<ExtAttribute> ExternalAttrs { get; set; }
    }
}