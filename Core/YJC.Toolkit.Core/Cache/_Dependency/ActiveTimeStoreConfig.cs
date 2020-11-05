using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheDependencyStoreConfig(NamespaceType = NamespaceType.Toolkit, Description = "活跃时间的缓存依赖",
        Author = "YJC", CreateDate = "2019-09-18", RegName = "ActiveTime")]
    internal class ActiveTimeStoreConfig : IConfigCreator<IDistributeCacheDependency>
    {
        [SimpleAttribute]
        public DateTime Current { get; set; }

        [SimpleAttribute]
        public TimeSpan Span { get; set; }

        public IDistributeCacheDependency CreateObject(params object[] args)
        {
            return new ActiveTimeDependency(this);
        }
    }
}