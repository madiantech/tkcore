using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Decoder
{
    [CacheInstance]
    class InternalSqlCodeTable : SqlCodeTable
    {
        public InternalSqlCodeTable(SqlCodeTableConfig config)
            : base(config)
        {
        }
    }
}
