using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class DbProviderPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_DbProvider";
        private const string DESCRIPTION = "数据库连接提供者的插件工厂";

        /// <summary>
        /// Initializes a new instance of the DbPlugInFactory class.
        /// </summary>
        public DbProviderPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
