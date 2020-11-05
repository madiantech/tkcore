using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public sealed class OperateRightPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_OperateRight";
        private const string DESCRIPTION = "操作权限的插件工厂";

        public OperateRightPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
