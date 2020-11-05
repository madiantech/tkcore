using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class ResolverPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_TableResolver";
        private const string DESCRIPTION = "数据表存取对象的插件工厂";

        public ResolverPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
