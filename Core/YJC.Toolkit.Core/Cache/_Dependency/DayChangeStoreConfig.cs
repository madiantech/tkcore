using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheDependencyStoreConfig(NamespaceType = NamespaceType.Toolkit, Description = "在自然日切换时无效的缓存依赖",
        Author = "YJC", CreateDate = "2019-09-18", RegName = "DayChange")]
    internal class DayChangeStoreConfig : IConfigCreator<IDistributeCacheDependency>
    {
        [SimpleAttribute]
        public DateTime CacheDay { get; set; }

        [SimpleAttribute]
        public int Days { get; set; }

        public IDistributeCacheDependency CreateObject(params object[] args)
        {
            return new DayChangeDependency(this);
        }
    }
}