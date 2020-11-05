using System;
using YJC.Toolkit.Sys;
using Toolkit.SchemaSuite;

namespace Toolkit.SchemaSuite.Data
{
    public class 请求恢复权利的理由
    {
        [SimpleElement(LocalName = "附加")]
        [TagElement(LocalName = "正当理由", Order = 10)]
        public string 正当理由 { get; private set; }
        
        [SimpleElement(LocalName = "附加")]
        [TagElement(LocalName = "不可抗拒的理由", Order = 20)]
        public string 不可抗拒的理由 { get; private set; }
        

        [SimpleElement(LocalName = "理由", Order = 30)]
        public string 理由 { get; private set; }
    }
}