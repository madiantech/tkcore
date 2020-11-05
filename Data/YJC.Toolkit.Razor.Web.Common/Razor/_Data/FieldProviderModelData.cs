using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class FieldProviderModelData : ProviderModelData
    {
        public FieldProviderModelData(IFieldValueProvider provider, IFieldInfoEx fieldInfo)
            : base(provider)
        {
            TkDebug.AssertArgumentNull(fieldInfo, "fieldInfo", null);

            FieldInfo = fieldInfo;
        }

        public IFieldInfoEx FieldInfo { get; private set; }
    }
}