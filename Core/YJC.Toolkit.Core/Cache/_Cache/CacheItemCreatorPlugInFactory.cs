using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    public sealed class CacheItemCreatorPlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_CacheItemCreator";
        private const string DESCRIPTION = "缓存项创建者的插件工厂";

        public CacheItemCreatorPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
