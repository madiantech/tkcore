using Microsoft.Extensions.Primitives;
using System;
using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Razor
{
    public class TemplateCacheItem : ICacheDependencyCreator
    {
        private readonly ICacheDependency fDependency;

        public TemplateCacheItem(string key, Func<ITemplatePage> pageFactory, IChangeToken token)
        {
            Key = key;
            TemplatePageFactory = pageFactory;
            fDependency = new ChangeTokenDependency(token);
        }

        public string Key { get; }

        public Func<ITemplatePage> TemplatePageFactory { get; }

        public ICacheDependency CreateCacheDependency()
        {
            return fDependency;
        }
    }
}