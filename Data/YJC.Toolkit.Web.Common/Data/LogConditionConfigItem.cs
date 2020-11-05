using YJC.Toolkit.Log;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class LogConditionConfigItem
    {
        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public InputConditionConfig Condition { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public LogConfigItem Log { get; private set; }
    }
}
