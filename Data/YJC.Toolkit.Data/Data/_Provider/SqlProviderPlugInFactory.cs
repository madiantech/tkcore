using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class SqlProviderPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_SqlProvider";
        private const string DESCRIPTION = "特定数据库SQL语句提供者的插件工厂";

        /// <summary>
        /// Initializes a new instance of the SqlProviderPlugInFactory class.
        /// </summary>
        public SqlProviderPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
