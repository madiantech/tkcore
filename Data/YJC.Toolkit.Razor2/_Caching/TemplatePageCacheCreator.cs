using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Cache;
using System.Threading.Tasks;

namespace YJC.Toolkit.Razor
{
    [InstancePlugIn, AlwaysCache]
    [CacheItemCreator(Author = "YJC", CreateDate = "2018-12-24", Description = "Razor 模板Page的缓存对象创建器")]
    internal class TemplatePageCacheCreator : BaseCacheItemCreator
    {
        internal static BaseCacheItemCreator Instance = new TemplatePageCacheCreator();

        private TemplatePageCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            var compiler = ObjectUtil.QueryObject<IRazorTemplateCompiler>(args);
            var factoryProvider = ObjectUtil.QueryObject<ITemplateFactoryProvider>(args);

            CompiledTemplateDescriptor templateDescriptor =
                Task.Run(async () => await compiler.CompileAsync(key)).GetAwaiter().GetResult();
            Func<ITemplatePage> templateFactory = factoryProvider.CreateFactory(templateDescriptor);

            TemplateCacheItem item = new TemplateCacheItem(key, templateFactory, templateDescriptor.ExpirationToken);
            return item;
        }
    }
}