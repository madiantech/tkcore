using YJC.Toolkit.Cache;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Decoder
{
    internal class NoCacheEasySearchProxyScheme : EasySearchProxyScheme
    {
        public NoCacheEasySearchProxyScheme(ITableScheme scheme, IDisplayObject displayObject)
            : base(scheme, displayObject)
        {
        }

        public override Cache.ICacheDependency CreateCacheDependency()
        {
            return NoDependency.Dependency;
        }
    }
}