using System;
using System.Configuration;
using System.IO;

namespace YJC.Toolkit.Sys
{
    public static class ConsoleApp
    {
        public static bool Initialize()
        {
            ConsoleGlobalVariable global = new ConsoleGlobalVariable();
            string appFile = string.Empty; //ConfigurationManager.AppSettings["applicationXml"];
            if (string.IsNullOrEmpty(appFile))
                appFile = @"../../../Xml/Application.xml";
            string startupPath = Environment.CurrentDirectory; //Application.StartupPath;
            if (!Path.IsPathRooted(appFile))
                appFile = Path.GetFullPath(Path.Combine(startupPath, appFile));

            ConsoleAppXml xml = new ConsoleAppXml();
            xml.ReadXmlFromFile(appFile);
            ConsoleAppSetting settings = new ConsoleAppSetting(startupPath, xml);
            bool continueRun;
            continueRun = !settings.Single || SingletonApp.Run();
            if (continueRun)
            {
                //Application.ApplicationExit += Application_ApplicationExit;
                AppDomain.CurrentDomain.ProcessExit += Application_ApplicationExit;
                global.Initialize(settings, null);
                if (settings.UseWorkThread)
                    global.CreateWorkThread();
            }
            return continueRun;
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            ConsoleGlobalVariable global = BaseGlobalVariable.Current.Convert<ConsoleGlobalVariable>();
            global.CloseWorkThread();
            BaseGlobalVariable.Current.Finalize(null);
        }
    }
}