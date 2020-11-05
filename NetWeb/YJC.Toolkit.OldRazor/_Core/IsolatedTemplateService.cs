using System;
using System.Collections.Generic;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class IsolatedTemplateService : MarshalByRefObject, ITemplateService
    {
        private static readonly Type TemplateServiceType = typeof(TemplateService);
        private static readonly BindingFlags fBindFlags = BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.Public;

        private readonly ITemplateService fProxy;
        private readonly AppDomain fAppDomain;
        private bool disposed;

        public IsolatedTemplateService()
            : this(RazorConfiguration.Current, null)
        {
        }

        public IsolatedTemplateService(RazorConfiguration config, IAppDomainFactory appDomainFactory)
        {
            config = config ?? RazorConfiguration.Current;
            fAppDomain = CreateAppDomain(appDomainFactory ?? new DefaultAppDomainFactory());

            string assemblyName = TemplateServiceType.Assembly.FullName;
            string typeName = TemplateServiceType.FullName;

            //string configJson = config.WriteJson();
            fProxy = (ITemplateService)fAppDomain.CreateInstance(
                assemblyName, typeName, false, fBindFlags,
                null, new object[] { config }, ObjectUtil.SysCulture, null).Unwrap();
        }

        #region ITemplateService 成员

        public ExecuteContext CreateExecuteContext(DynamicObjectBag viewBag)
        {
            throw new NotSupportedException("This operation is not supported directly by the IsolatedTemplateService.");
        }

        public RazorBaseTemplate CreateTemplate(string razorTemplate, Type templateType, Type baseType,
            string className, object model, DynamicObjectBag viewBag, object configurationData)
        {
            if (disposed)
                throw new ObjectDisposedException("IsolatedTemplateService");

            if (model != null)
            {
                //if (CompilerServicesUtility.IsDynamicType(model.GetType()))
                //    throw new ArgumentException("IsolatedTemplateService instances do not support anonymous or dynamic types.");
            }

            return fProxy.CreateTemplate(razorTemplate, templateType, baseType, className, 
                model, viewBag, configurationData);
        }

        public Type CreateTemplateType(string razorTemplate, Type templateType, string className, 
            IEnumerable<string> assemblies)
        {
            if (disposed)
                throw new ObjectDisposedException("IsolatedTemplateService");

            return fProxy.CreateTemplateType(razorTemplate, templateType, className, assemblies);
        }

        public RazorBaseTemplate GetTemplate(string razorTemplate, Type baseType, object model, 
            DynamicObjectBag viewBag,  string cacheName, object configurationData)
        {
            if (disposed)
                throw new ObjectDisposedException("IsolatedTemplateService");

            return fProxy.GetTemplate(razorTemplate, baseType, model, viewBag, cacheName, configurationData);
        }

        public bool HasTemplate(string cacheName)
        {
            return fProxy.HasTemplate(cacheName);
        }

        public bool RemoveTemplate(string cacheName)
        {
            return fProxy.RemoveTemplate(cacheName);
        }

        public string Parse(Type baseType, string razorTemplate, object model,
            DynamicObjectBag viewBag, string cacheName, object configurationData)
        {
            if (disposed)
                throw new ObjectDisposedException("IsolatedTemplateService");

            if (model != null)
            {
                //if (CompilerServicesUtility.IsDynamicType(model.GetType()))
                //    throw new ArgumentException("IsolatedTemplateService instances do not support anonymous or dynamic types.");
            }

            return fProxy.Parse(baseType, razorTemplate, model, viewBag, cacheName, configurationData);
        }

        public RazorBaseTemplate Resolve(string cacheName, object model, 
            DynamicObjectBag viewBag, object configurationData)
        {
            return fProxy.Resolve(cacheName, model, viewBag, configurationData);
        }

        public string Run(RazorBaseTemplate razorTemplate, DynamicObjectBag viewBag)
        {
            return fProxy.Run(razorTemplate, viewBag);
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                fProxy.Dispose();

                AppDomain.Unload(fAppDomain);
                disposed = true;
            }
        }

        private static AppDomain CreateAppDomain(IAppDomainFactory factory)
        {
            var domain = factory.CreateAppDomain();

            if (domain == null)
                throw new InvalidOperationException("The application domain factory did not create an application domain.");

            if (domain == AppDomain.CurrentDomain)
                throw new InvalidOperationException("The application domain factory returned the current application domain which cannot be used for isolation.");

            return domain;
        }
    }
}
