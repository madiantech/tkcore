using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class StatFieldConfigItem
    {
        [SimpleAttribute(Required = true)]
        public string NickName { get; private set; }

        [SimpleAttribute(DefaultValue = StatMethod.Sum)]
        public StatMethod Method { get; private set; }
    }
}