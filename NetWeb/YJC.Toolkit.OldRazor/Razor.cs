using System;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public static partial class RazorUtil
    {
        private static ITemplateService fService = new TemplateService();
        private static readonly object fSync = new object();

        private static ITemplateService TemplateService
        {
            get
            {
                lock (fSync)
                    return fService;
            }
        }

        public static string Parse(string razorTemplate)
        {
            return TemplateService.Parse(null, razorTemplate, null, null, null, null);
        }

        public static string Parse(string razorTemplate, string cacheName)
        {
            return TemplateService.Parse(null, razorTemplate, null, null, cacheName, null);
        }

        public static string Parse(string razorTemplate, object model)
        {
            return TemplateService.Parse(null, razorTemplate, model, null, null, null);
        }

        public static string Parse(string razorTemplate, object model, string cacheName)
        {
            return TemplateService.Parse(null, razorTemplate, model, null, cacheName, null);
        }

        public static string Parse(string razorTemplate, object model,
            DynamicObjectBag viewBag, string cacheName)
        {
            return TemplateService.Parse(null, razorTemplate, model, viewBag, cacheName, null);
        }

        public static string Parse(Type baseType, string razorTemplate, object model,
            DynamicObjectBag viewBag, string cacheName, object configurationData)
        {
            return TemplateService.Parse(baseType, razorTemplate, model, viewBag, cacheName, configurationData);
        }

        public static string ParseFromFile(string fileName, object model, DynamicObjectBag viewBag)
        {
            return ParseFromFile(null, fileName, null, model, viewBag, null);
        }


        public static string ParseFromFile(Type baseType, string fileName, Encoding encoding,
            object model, DynamicObjectBag viewBag, object configurationData)
        {
            return ParseFromFile(TemplateService, baseType, fileName, encoding, model,
                viewBag, configurationData);
        }

        internal static string ParseFromFile(ITemplateService service, Type baseType, string fileName,
            Encoding encoding, object model, DynamicObjectBag viewBag, object configurationData)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);

            return ParseFromFile(service, baseType, fileName, null, encoding, model, viewBag, configurationData);
        }

        public static string ParseFromFile(Type baseType, string fileName, string layoutFile,
            Encoding encoding, object model, DynamicObjectBag viewBag, object configurationData)
        {
            return ParseFromFile(TemplateService, baseType, fileName, layoutFile,
                encoding, model, viewBag, configurationData);
        }

        internal static string ParseFromFile(ITemplateService service, Type baseType,
            string fileName, string layoutFile, Encoding encoding, object model,
            DynamicObjectBag viewBag, object configurationData)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);

            FileData data = new FileData(baseType, fileName, layoutFile, encoding, configurationData);
            return service.Parse(data.BaseType, data.RazorTemplate, model, viewBag,
                data.CacheName, data.ConfigurationData);
        }

        public static void SetTemplateService(ITemplateService service)
        {
            TkDebug.AssertArgumentNull(service, "service", null);

            lock (fSync)
                fService = service;
        }
    }
}
