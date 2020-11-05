using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Decoder
{
    [CacheInstance]
    class InternalTk5DbCodeTable : Tk5DbCodeTable
    {
        public InternalTk5DbCodeTable(Tk5CodeTableConfig config)
            : base(config)
        {
        }
    }
}
