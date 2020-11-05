using YJC.Toolkit.Cache;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2013-10-08",
        Description = "单表Scheme的分析数据的缓存对象创建器")]
    [InstancePlugIn, AlwaysCache]
    internal sealed class TableSchemeDataCacheCreator : BaseCacheItemCreator
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public static BaseCacheItemCreator Instance = new TableSchemeDataCacheCreator();

        private TableSchemeDataCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            TkDbContext context;
            ITableScheme scheme = ObjectUtil.QueryObject<ITableScheme>(args);
            TkDebug.AssertNotNull(scheme, "参数中缺少ITableScheme类型", this);
            if (args.Length == 2)
                context = ObjectUtil.QueryObject<TkDbContext>(args);
            else
                context = null;
            return new TableSchemeData(context, scheme);
        }
    }
}
