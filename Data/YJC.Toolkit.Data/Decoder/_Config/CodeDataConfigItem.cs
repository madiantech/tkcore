using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class CodeDataConfigItem : BaseXmlPlugInItem
    {
        public const string CODE_DATA_BASE_CLASS = "CodeData";

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Row", Required = true)]
        public List<RowConfigItem> RowList { get; private set; }

        #region IXmlPlugInItem 成员

        public override string BaseClass
        {
            get
            {
                return CODE_DATA_BASE_CLASS;
            }
        }

        #endregion
    }
}