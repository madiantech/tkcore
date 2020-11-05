using System;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.HtmlExtension
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2018-10-08", RegName = HtmlUtil.CACHE_NAME,
        Description = "单表Scheme的分析数据的缓存对象创建器")]
    [InstancePlugIn, AlwaysCache]
    internal sealed class HtmlCacheCreator : BaseCacheItemCreator
    {
        public static readonly BaseCacheItemCreator Instance = new HtmlCacheCreator();

        private HtmlCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            var param = ObjectUtil.QueryObject<Tuple<string, string, HtmlOption>>(args);
            string fileName = param.Item1;
            string virtualPath = param.Item2;
            var option = param.Item3;
            string cacheFileName = HtmlUtil.GetCacheFile(HtmlUtil.GetCacheKey(fileName, virtualPath));
            return new HtmlData(fileName, cacheFileName, virtualPath, option);
        }

        public override string TransformCacheKey(string key)
        {
            return key.ToLower(ObjectUtil.SysCulture);
        }
    }
}