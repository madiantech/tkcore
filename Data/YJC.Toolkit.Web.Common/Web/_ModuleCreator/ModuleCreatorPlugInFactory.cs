using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class ModuleCreatorPlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_ModuleCreator";
        private const string DESCRIPTION = "用特定方式生成Module的插件工厂";

        public ModuleCreatorPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
