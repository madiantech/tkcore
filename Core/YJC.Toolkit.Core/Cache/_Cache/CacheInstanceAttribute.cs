using System;

namespace YJC.Toolkit.Cache
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class CacheInstanceAttribute : Attribute
    {
        public override string ToString()
        {
            return "标记缓存该类型插件的特性";
        }
    }
}
