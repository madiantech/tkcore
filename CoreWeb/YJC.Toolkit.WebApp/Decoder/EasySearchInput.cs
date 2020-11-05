using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class EasySearchInput
    {
        [SimpleAttribute]
        public string RegName { get; private set; }

        [SimpleAttribute]
        public string Text { get; private set; }

        [ObjectElement(IsMultiple = true, LocalName = "RefField")]
        public List<EasySearchRefField> RefFields { get; private set; }
    }
}
