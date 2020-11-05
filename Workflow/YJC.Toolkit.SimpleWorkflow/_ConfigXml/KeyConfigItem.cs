using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class KeyConfigItem
    {
        [SimpleAttribute(Required = true)]
        public string NickName { get; private set; }

        [SimpleAttribute(Required = true)]
        public string ParamName { get; private set; }
    }
}