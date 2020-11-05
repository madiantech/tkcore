using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class EasySearchConfigItem : DecoderConfigItem
    {
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "RefValue",
            UseConstructor = true)]
        public List<EasySearchRefConfig> RefFields { get; private set; }
    }
}