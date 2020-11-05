using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public sealed class CodeTableContainer
    {
        private readonly Dictionary<string, IEnumerable<IDecoderItem>> fData;

        public CodeTableContainer()
        {
            fData = new Dictionary<string, IEnumerable<IDecoderItem>>();
        }

        public void Add(string regName, IEnumerable<IDecoderItem> codeItems)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", this);
            TkDebug.AssertArgumentNull(codeItems, "codeItems", this);

            if (!fData.ContainsKey(regName))
                fData.Add(regName, codeItems);
        }

        public IEnumerable<IDecoderItem> this[string regName]
        {
            get
            {
                TkDebug.AssertArgumentNullOrEmpty(regName, "regName", this);

                return ObjectUtil.TryGetValue(fData, regName);
            }
        }
    }
}
