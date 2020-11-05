using System;
using System.Collections.Generic;
using System.Text;

namespace YJC.Toolkit.Cache
{
    public abstract class DistributeCacheItemCreator<T> : BaseDistributeCacheItemCreator<T> where T : new()
    {
        protected DistributeCacheItemCreator()
        {
        }

        protected DistributeCacheItemCreator(int capacity) : base(capacity)
        {
        }

        protected DistributeCacheItemCreator(ICacheCreator cacheCreator) : base(cacheCreator)
        {
        }

        public override object CreateEmptyData()
        {
            return new T();
        }
    }
}