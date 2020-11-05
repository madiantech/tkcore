using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class DecoderConfigItem
    {
        [SimpleAttribute(Required = true)]
        public string RegName { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true,
            LocalName = "AdditionInfo", UseConstructor = true)]
        public List<DecoderAdditionInfo> AdditionInfos { get; protected set; }
    }
}
