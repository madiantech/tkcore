using System;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace YJC.Toolkit.Sys
{
    public static class WebApp
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
#if NETCOREAPP2_2
        public static void ApplicationStart(in Microsoft.AspNetCore.Hosting.IHostingEnvironment application,
            in string applicationFile, in ILogger<ToolkitService> logger)
#endif
#if NETCOREAPP3_1
        public static void ApplicationStart(in IWebHostEnvironment application, in string applicationFile,
            in ILogger<ToolkitService> logger)
#endif
        {
            WebGlobalVariable global = new WebGlobalVariable();
            //string appFile = ConfigurationManager.AppSettings["applicationXml"];
            global.Trace = new ToolkitWebTrace(logger);
            string appFile = applicationFile;
            if (string.IsNullOrEmpty(appFile))
                appFile = @"Xml/Application.xml";
            string startupPath = Path.GetFullPath(Path.Combine(application.WebRootPath, ".."));
            if (!Path.IsPathRooted(appFile))
                appFile = Path.GetFullPath(Path.Combine(startupPath, appFile));

            WebAppXml xml = new WebAppXml();
            xml.ReadXmlFromFile(appFile);
            WebAppSetting settings = new WebAppSetting(startupPath, xml)
            {
                IsDevelopment = application.IsDevelopment()
                //StartUrl = UriUtil.GetBaseUri(application.Context.Request.Url)
            };
            settings.SetPath(xml);
            //global.InitialIndexer();
            global.Initialize(settings, application);
            if (settings.UseWorkThread)
                global.CreateWorkThread();

            settings.Config(global.WebDefaultValue);
            //global.ErrorLog.Flush();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
#if NETCOREAPP2_2
        public static void ApplicationEnd(Microsoft.AspNetCore.Hosting.IHostingEnvironment application)
#endif
#if NETCOREAPP3_1
        public static void ApplicationEnd(IWebHostEnvironment application)
#endif
        {
            //GlobalVariable.Save();
            BaseGlobalVariable.Current.Finalize(application);
        }
    }
}