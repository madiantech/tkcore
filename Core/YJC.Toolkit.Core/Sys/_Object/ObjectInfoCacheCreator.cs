using System;
using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Sys
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2013-09-29",
        Description = "ObjectInfo的缓存对象创建器")]
    [InstancePlugIn, AlwaysCache]
    internal class ObjectInfoCacheCreator : BaseCacheItemCreator
    {
        internal static BaseCacheItemCreator Instance = new ObjectInfoCacheCreator();

        private ObjectInfoCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            object data = args[0];
            string modeName = (string)args[1];

            ObjectInfo result;
            Type dataType = data as Type;
            if (dataType != null)
                result = new ObjectInfo(null, dataType, modeName);
            else
                result = new ObjectInfo(data, data.GetType(), modeName);

            return result;
        }
    }
}
