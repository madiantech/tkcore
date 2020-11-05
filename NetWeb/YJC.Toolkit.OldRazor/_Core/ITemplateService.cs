using System;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    /// <summary>
    /// Defines the required contract for implementing a template service.
    /// </summary>
    public interface ITemplateService : IDisposable
    {
        /// <summary>
        /// Gets the template service configuration.
        /// </summary>
        //ITemplateServiceConfiguration Configuration { get; }

        ExecuteContext CreateExecuteContext(DynamicObjectBag viewBag);

        RazorBaseTemplate CreateTemplate(string razorTemplate, Type templateType, Type baseType,
            string className, object model, DynamicObjectBag viewBag, object configurationData);

        Type CreateTemplateType(string razorTemplate, Type templateType, string className, 
            IEnumerable<string> assemblies);

        RazorBaseTemplate GetTemplate(string razorTemplate, Type baseType,
            object model, DynamicObjectBag viewBag, string cacheName, object configurationData);

        bool HasTemplate(string cacheName);

        bool RemoveTemplate(string cacheName);

        string Parse(Type baseType, string razorTemplate, object model,
            DynamicObjectBag viewBag, string cacheName, object configurationData);

        RazorBaseTemplate Resolve(string cacheName, object model, DynamicObjectBag viewBag, object configurationData);

        string Run(RazorBaseTemplate razorTemplate, DynamicObjectBag viewBag);
    }
}