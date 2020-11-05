using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public static class DbContextUtil
    {
        private static ISupportDbContext GetSupport()
        {
            TkDebug.ThrowIfNoAppSetting();

            ISupportDbContext support = BaseAppSetting.Current as ISupportDbContext;
            TkDebug.AssertNotNull(support, "AppSetting不支持ISupportDbContext接口，无法创建DbContext",
                BaseAppSetting.Current);
            return support;
        }

        public static DbContextConfig GetDbContextConfig(string name)
        {
            ISupportDbContext support = GetSupport();
            if (string.IsNullOrEmpty(name))
                return support.Default;
            else
                return support.GetContextConfig(name);
        }

        public static TkDbContext CreateDefault()
        {
            ISupportDbContext support = GetSupport();
            DbContextConfig config = support.Default;
            TkDebug.AssertNotNull(config, "AppSetting中没有配置Default的DbContext", support);
            return config.CreateDbContext();
        }

        public static TkDbContext CreateDbContext(string name)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", null);

            ISupportDbContext support = GetSupport();
            DbContextConfig config = support.GetContextConfig(name);
            TkDebug.AssertNotNull(config, string.Format(ObjectUtil.SysCulture,
                "AppSetting中没有配置名称为{0}的DbContext", name), support);

            return config.CreateDbContext();
        }
    }
}