using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class TreePlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_Tree";
        private const string DESCRIPTION = "生成Tree结构对象的插件工厂";

        public TreePlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
