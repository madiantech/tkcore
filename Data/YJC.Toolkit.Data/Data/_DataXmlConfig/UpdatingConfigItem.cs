using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class UpdatingConfigItem : MarcoConfigItem
    {
        [SimpleAttribute]
        public UpdatingUpdateKind UpdateKind { get; private set; }
    }
}