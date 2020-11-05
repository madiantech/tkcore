using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class StatConfigItem
    {
        [SimpleAttribute(DefaultValue = StatCalcMode.Last)]
        public StatCalcMode CalcMode { get; private set; }

        [SimpleAttribute]
        public bool UseSubTotal { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true, IsMultiple = true, LocalName = "StatField")]
        public List<StatFieldConfigItem> StatFieldList { get; private set; }
    }
}