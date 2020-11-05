using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class UpdatedActionPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_UpdatedAction";
        private const string DESCRIPTION = "UpdatedAction插件工厂";

        public UpdatedActionPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}