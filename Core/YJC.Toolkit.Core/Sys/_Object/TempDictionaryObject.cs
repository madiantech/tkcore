using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    internal sealed class TempDictionaryObject
    {
        [Dictionary]
        public Dictionary<string, string> Temp { get; set; }
    }
}
