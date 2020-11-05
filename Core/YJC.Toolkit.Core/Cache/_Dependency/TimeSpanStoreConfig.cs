using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheDependencyStoreConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2019-09-19",
        Description = "一段时间内有效的缓存依赖", Author = "YJC", RegName = "TimeSpan")]
    internal class TimeSpanStoreConfig : IConfigCreator<IDistributeCacheDependency>
    {
        [SimpleAttribute]
        public DateTime ExpireTime { get; set; }

        public IDistributeCacheDependency CreateObject(params object[] args)
        {
            return new TimeSpanDependency(this);
        }
    }
}