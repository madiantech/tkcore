using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal sealed class DbProviderTypeConverter : BasePlugInTypeConverter<IDbProvider>
    {
        public DbProviderTypeConverter()
            : base(DbProviderPlugInFactory.REG_NAME)
        {
        }
    }
}
