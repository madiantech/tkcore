using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace Toolkit.SchemaSuite
{
    public class SectionGroup
    {
        public SectionGroup(int order, string caption)
        {
            Order = order;
            Caption = caption;
        }

        [SimpleAttribute(Required = true)]
        public int Order { get; private set; }

        [SimpleAttribute]
        public bool IsCollaspe { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public string Caption { get; private set; }

        [SimpleAttribute]
        public int EndOrder { get; internal set; }
    }
}