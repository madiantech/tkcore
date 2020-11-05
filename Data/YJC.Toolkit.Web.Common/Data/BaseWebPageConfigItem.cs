using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class BaseWebPageConfigItem
    {
        [SimpleAttribute(DefaultValue = true)]
        public bool SupportLogOn { get; protected set; }

        [SimpleAttribute]
        public bool DisableInjectCheck { get; protected set; }

        [SimpleAttribute]
        public bool CheckSubmit { get; protected set; }
    }
}