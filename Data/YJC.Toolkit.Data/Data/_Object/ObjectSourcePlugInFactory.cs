using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class ObjectSourcePlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_ObjectSource";
        private const string DESCRIPTION = "基于对象的数据源的插件工厂";

        public ObjectSourcePlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
