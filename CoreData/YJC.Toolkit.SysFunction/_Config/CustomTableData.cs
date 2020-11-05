using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SysFunction
{
    class CustomTableData
    {
        [SimpleAttribute]
        public string TableName { get; private set; }

        [SimpleAttribute]
        public string DisplayName { get; private set; }

        [SimpleAttribute]
        public string NameField { get; private set; }

        [ObjectElement]
        public ResolverConfig Extension1 { get; private set; }

    }
}
