using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Sys
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2014-06-03",
       Description = "文件内容缓存器")]
    [InstancePlugIn, AlwaysCache]
    internal sealed class FileContentCacheCreator : BaseDistributeCacheItemCreator<FileData>
    {
        internal static BaseCacheItemCreator Instance = new FileContentCacheCreator();

        private FileContentCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            return new FileData(key);
        }
    }
}