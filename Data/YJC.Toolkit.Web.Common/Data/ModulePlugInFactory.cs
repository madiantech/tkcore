using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ModulePlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_Module";
        private const string DESCRIPTION = "功能定义的插件工厂";

        public ModulePlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
