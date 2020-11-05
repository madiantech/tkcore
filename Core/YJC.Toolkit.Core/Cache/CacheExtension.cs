using System;
using System.Collections.Generic;
using System.Text;

namespace YJC.Toolkit.Cache
{
    public static class CacheExtension
    {
        public static DateTimeOffset? GetAbsoluteExpiration(this ICacheDependency dependency)
        {
            if (dependency is ICacheDependencyTime dependencyTime)
                return new DateTimeOffset(dependencyTime.AbsoluteExpiration);

            return null;
        }
    }
}