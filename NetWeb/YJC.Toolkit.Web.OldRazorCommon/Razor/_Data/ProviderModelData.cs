using System;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class ProviderModelData
    {
        public ProviderModelData(IFieldValueProvider provider)
        {
            TkDebug.AssertArgumentNull(provider, "provider", null);

            Provider = provider;
        }

        public IFieldValueProvider Provider { get; private set; }
    }
}