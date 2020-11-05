using System;
using YJC.Toolkit.Sys;
using Toolkit.SchemaSuite;

namespace Toolkit.SchemaSuite.Data
{
    public class 专利申请或专利
    {

        [SimpleElement(LocalName = "申请号或专利号", Order = 10)]
        public string 申请号或专利号 { get; private set; }

        [SimpleElement(LocalName = "发明创造名称", Order = 20)]
        public string 发明创造名称 { get; private set; }

        [SimpleElement(LocalName = "申请人或专利权人姓名或名称", Order = 30)]
        public string 申请人或专利权人姓名或名称 { get; private set; }
    }
}