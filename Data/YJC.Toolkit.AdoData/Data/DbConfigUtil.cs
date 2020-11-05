using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal static class DbConfigUtil
    {
        public static TableResolver CreateSingleTableResolver(object config, IDbDataSource source)
        {
            TkDebug.AssertArgumentNull(config, "config", null);
            TkDebug.AssertArgumentNull(source, "source", null);

            ISingleResolverConfig intf = config as ISingleResolverConfig;
            if (intf != null)
                return intf.Resolver.CreateObject(source);

            IMultipleResolverConfig multiple = config as IMultipleResolverConfig;
            if (multiple != null)
                return multiple.MainResolver.CreateObject(source);

            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                "{0}即没有支持ISingleResolverConfig接口，也没有支持IMultipleResolverConfig接口，无法创建TableResolver",
                config.GetType()), config);
            return null;
        }
    }
}
