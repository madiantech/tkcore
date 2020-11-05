using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    class SqlSourceSqlConfig : MarcoConfigItem
    {
        [SimpleAttribute]
        public string TableName { get; private set; }
    }
}
