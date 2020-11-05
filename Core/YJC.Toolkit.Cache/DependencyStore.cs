using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    internal class DependencyStore
    {
        [DynamicElement(CacheDependencyStoreConfigFactory.REG_NAME)]
        public object StoreObject { get; set; }
    }
}