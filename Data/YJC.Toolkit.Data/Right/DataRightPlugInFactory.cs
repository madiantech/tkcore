using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class DataRightPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_DataRight";
        internal const string DESCRIPTION = "数据权限插件工厂";

        public DataRightPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
