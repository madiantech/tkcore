using System.Collections.Generic;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class EasySearchList
    {
        private readonly Dictionary<string, EasySearchContainer> fData;

        public EasySearchList()
        {
            fData = new Dictionary<string, EasySearchContainer>();
        }

        public void Add(string regName, EasySearch easySearch)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", this);
            TkDebug.AssertArgumentNull(easySearch, "easySearch", this);

            if (!fData.ContainsKey(regName))
                fData.Add(regName, new EasySearchContainer(easySearch));
        }

        public EasySearch GetEasySearch(string regName, ControlType ctrlType)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", this);

            EasySearchContainer result = ObjectUtil.TryGetValue(fData, regName);
            return result == null ? null : result.GetEasySearch(ctrlType);
        }

        public bool ContainsKey(string regName)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", this);

            return fData.ContainsKey(regName);
        }

        public void Clear()
        {
            fData.Clear();
        }
    }
}
