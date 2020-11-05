using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal sealed class SqlProviderTypeConverter : BasePlugInTypeConverter<ISqlProvider>
    {
        public SqlProviderTypeConverter()
            : base(SqlProviderPlugInFactory.REG_NAME)
        {
        }
    }
}
