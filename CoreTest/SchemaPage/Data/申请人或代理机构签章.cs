using System;
using YJC.Toolkit.Sys;
using Toolkit.SchemaSuite;

namespace Toolkit.SchemaSuite.Data
{
    public class 申请人或代理机构签章
    {

        [SimpleElement(LocalName = "签章", Order = 10)]
        public string 签章 { get; private set; }

        [ObjectElement(LocalName = "签章日期", Order = 20)]
        public DateClass 签章日期 { get; private set; }
    }
}