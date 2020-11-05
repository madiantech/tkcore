using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class DisplayPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_Display";
        private const string DESCRIPTION = "字段显示的插件工厂";

        public DisplayPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
