using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Decoder
{
    [CacheInstance]
    [DayChangeCache]
    class InternalStandardDbCodeTable : StandardDbCodeTable
    {
        public InternalStandardDbCodeTable(StandardCodeTableConfig config)
            : base(config)
        {
        }
    }
}
