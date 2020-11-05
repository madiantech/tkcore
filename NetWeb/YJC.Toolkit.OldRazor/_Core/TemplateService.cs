using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class TemplateService : MarshalByRefObject, ITemplateService
    {
        private readonly RazorConfiguration fConfig;

        private readonly ConcurrentDictionary<string, CachedTemplateItem> fCache
            = new ConcurrentDictionary<string, CachedTemplateItem>();

        private readonly ConcurrentDictionary<Type, TemplateTypeItem> fTypeCache
            = new ConcurrentDictionary<Type, TemplateTypeItem>();

        public TemplateService()
            : this(RazorConfiguration.Current)
        {
        }

        public TemplateService(RazorConfiguration config)
        {
            fConfig = config;
        }

        //internal TemplateService(ITemplateService service, RazorConfiguration config)
        //{
        //    fConfig = config;
        //    Razor.SetTemplateService(this);
        //}

        #region ITemplateService 成员

        public ExecuteContext CreateExecuteContext(DynamicObjectBag viewBag)
        {
            var context = new ExecuteContext(new DynamicObjectBag(viewBag)) { Config = fConfig };

            return context;
        }

        public RazorBaseTemplate CreateTemplate(string razorTemplate, Type templateType, Type baseType,
            string className, object model, DynamicObjectBag viewBag, object configurationData)
        {
            TkDebug.AssertArgumentNullOrEmpty(razorTemplate, "razorTemplate", this);

            IEnumerable<string> assembies;
            if (viewBag != null)
                assembies = ((dynamic)viewBag).Assembly;
            else
                assembies = null;
            if (templateType == null)
                templateType = CreateTemplateType(razorTemplate, baseType, className, assembies);

            RazorBaseTemplate instance = ObjectUtil.CreateObject(templateType).Convert<RazorBaseTemplate>();
            instance.TemplateService = this;
            instance.Model = model;

            if (configurationData != null)
                instance.InitializeTemplate(configurationData);

            return instance;
        }

        public Type CreateTemplateType(string razorTemplate, Type templateType, string className,
            IEnumerable<string> assemblies)
        {
            TkDebug.AssertArgumentNull(razorTemplate, "razorTemplate", this);

            if (templateType == null)
                templateType = typeof(RazorBaseTemplate);
            string cacheName = className;
            if (string.IsNullOrEmpty(className))
                className = RazorUtil.GenerateClassName();
            else
                className = RazorUtil.GenerateClassName(className);

            TemplateTypeItem item = fTypeCache.GetOrAdd(templateType,
                (type) => new TemplateTypeItem(type));
            var complieAssemblies = EnumUtil.Convert(item.Assemblies, assemblies).Distinct();
            Assembly assembly = CompilerService.ComplieToAssembly(templateType, razorTemplate,
                className, cacheName, item.Namespaces, complieAssemblies, fConfig);

            Type resultType = assembly.GetType(string.Format(ObjectUtil.SysCulture, "{0}.{1}",
                CompilerService.DEFAULT_NS, className), false);
            return resultType;
        }

        public RazorBaseTemplate GetTemplate(string razorTemplate, Type baseType, object model,
            DynamicObjectBag viewBag, string cacheName, object configurationData)
        {
            TkDebug.AssertArgumentNullOrEmpty(razorTemplate, "razorTemplate", this);
            TkDebug.AssertArgumentNullOrEmpty(cacheName, "cacheName", this);

            int hashCode = razorTemplate.GetHashCode();

            CachedTemplateItem item;
            if (!(fCache.TryGetValue(cacheName, out item) && item.CachedHashCode == hashCode))
            {
                IEnumerable<string> assembies;
                if (viewBag != null)
                    assembies = ((dynamic)viewBag).Assembly;
                else
                    assembies = null;
                Type type = CreateTemplateType(razorTemplate, baseType, cacheName, assembies);
                item = new CachedTemplateItem(hashCode, type);

                fCache.AddOrUpdate(cacheName, item, (n, i) => item);
            }

            var instance = CreateTemplate(razorTemplate, item.TemplateType, baseType,
                cacheName, model, viewBag, configurationData);
            return instance;
        }

        public bool HasTemplate(string cacheName)
        {
            return fCache.ContainsKey(cacheName);
        }

        public bool RemoveTemplate(string cacheName)
        {
            CachedTemplateItem item;
            return fCache.TryRemove(cacheName, out item);
        }

        public string Parse(Type baseType, string razorTemplate, object model,
            DynamicObjectBag viewBag, string cacheName, object configurationData)
        {
            TkDebug.AssertArgumentNullOrEmpty(razorTemplate, "razorTemplate", this);

            RazorBaseTemplate instance;
            if (string.IsNullOrEmpty(cacheName))
                instance = CreateTemplate(razorTemplate, null, baseType, null, model,
                    viewBag, configurationData);
            else
                instance = GetTemplate(razorTemplate, baseType, model, viewBag, cacheName,
                    configurationData);

            return Run(instance, viewBag);
        }

        public RazorBaseTemplate Resolve(string cacheName, object model, DynamicObjectBag viewBag,
            object configurationData)
        {
            CachedTemplateItem cachedItem;
            RazorBaseTemplate instance;
            if (fCache.TryGetValue(cacheName, out cachedItem))
                instance = CreateTemplate(null, cachedItem.TemplateType, null, null,
                    model, viewBag, configurationData);
            else
                instance = null;

            //if (instance == null && _config.Resolver != null)
            //{
            //    string template = _config.Resolver.Resolve(cacheName);
            //    if (!string.IsNullOrWhiteSpace(template))
            //        instance = GetTemplate(template, model, cacheName);
            //}

            return instance;
        }

        public string Run(RazorBaseTemplate razorTemplate, DynamicObjectBag viewBag)
        {
            TkDebug.AssertArgumentNull(razorTemplate, "razorTemplate", this);

            return razorTemplate.Run(CreateExecuteContext(viewBag));
        }

        #endregion ITemplateService 成员

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}