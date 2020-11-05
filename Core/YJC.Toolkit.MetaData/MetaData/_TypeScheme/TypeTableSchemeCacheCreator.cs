using System;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2014-06-05",
        Description = "基于类型的单表Scheme的缓存对象创建器")]
    [InstancePlugIn]
    class TypeTableSchemeCacheCreator : BaseCacheItemCreator
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal static BaseCacheItemCreator Instance = new TypeTableSchemeCacheCreator();

        private TypeTableSchemeCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            Type type = ObjectUtil.ConfirmQueryObject<Type>(this, args);

            return new TypeTableScheme(type);
        }
    }
}
