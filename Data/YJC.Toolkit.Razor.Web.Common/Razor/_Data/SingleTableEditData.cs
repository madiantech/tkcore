using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class SingleTableEditData : SingleTableDetailData
    {
        [SimpleAttribute]
        public string OnRowAdded { get; private set; }

        [SimpleAttribute]
        public string OnRowDeleted { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public RazorOutputData RowDisplay { get; private set; }
    }
}