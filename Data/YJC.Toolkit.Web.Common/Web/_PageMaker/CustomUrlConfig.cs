using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public sealed class CustomUrlConfig : MarcoConfigItem
    {
        public CustomUrlConfig()
        {
        }

        public CustomUrlConfig(bool needParse, bool sqlInject, string value)
            : base(needParse, sqlInject, value)
        {
        }

        public CustomUrlConfig(bool needParse, bool sqlInject, string value, string emptyMarco)
            : base(needParse, sqlInject, value, emptyMarco)
        {
        }

        [SimpleAttribute]
        public bool UseKeyData { get; internal set; }
    }
}