using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class SourcePlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_Source";
        private const string DESCRIPTION = "数据源的插件工厂";

        public SourcePlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
