using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite
{
    public class DateClass
    {
        [SimpleElement(LocalName = "年")]
        public string Year { get; private set; }

        [SimpleElement(LocalName = "月")]
        public string Month { get; private set; }

        [SimpleElement(LocalName = "日")]
        public string Day { get; private set; }
    }
}