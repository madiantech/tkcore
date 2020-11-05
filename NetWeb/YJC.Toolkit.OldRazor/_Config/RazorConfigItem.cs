using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class RazorConfigItem
    {
        [SimpleAttribute(DefaultValue = true)]
        public bool RaiseOnCompileError { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool RaiseOnRunError { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool SaveCompileCode { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool SaveCompileAssembly { get; private set; }

        [SimpleAttribute]
        public RazorServiceType Service { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public SavePathConfigItem SavePath { get; private set; }
    }
}