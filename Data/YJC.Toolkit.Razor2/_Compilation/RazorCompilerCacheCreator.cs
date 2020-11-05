using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [InstancePlugIn, AlwaysCache]
    [CacheItemCreator(Author = "YJC", CreateDate = "2018-12-25", Description = "Razor代码编译的缓存对象创建器")]
    internal class RazorCompilerCacheCreator : BaseCacheItemCreator
    {
        internal static BaseCacheItemCreator Instance = new RazorCompilerCacheCreator();

        private RazorCompilerCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            RazorTemplateCompiler compiler = ObjectUtil.QueryObject<RazorTemplateCompiler>(args);
            return compiler.CreateDescriptor(key);
        }
    }
}