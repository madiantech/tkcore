using System;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.SessionState;

namespace YJC.Toolkit.Sys
{
    public static class WebApp
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void ApplicationStart(HttpApplication application)
        {
            WebGlobalVariable global = new WebGlobalVariable();
            string appFile = ConfigurationManager.AppSettings["applicationXml"];
            if (string.IsNullOrEmpty(appFile))
                appFile = @"..\Xml\Application.xml";
            string startupPath = HttpRuntime.AppDomainAppPath;
            if (!Path.IsPathRooted(appFile))
                appFile = Path.GetFullPath(Path.Combine(startupPath, appFile));

            WebAppXml xml = new WebAppXml();
            xml.ReadXmlFromFile(appFile);
            WebAppSetting settings = new WebAppSetting(startupPath, xml)
            {
                //StartUrl = UriUtil.GetBaseUri(application.Context.Request.Url)
            };
            settings.SetPath(xml);
            global.InitialIndexer();
            global.Initialize(settings, application);
            if (settings.UseWorkThread)
                global.CreateWorkThread();
            WebAppExtensionXml extXml = new WebAppExtensionXml();
            extXml.ReadXmlFromFile(appFile);
            settings.Config(extXml);
            //global.ErrorLog.Flush();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void ApplicationEnd(HttpApplication application)
        {
            //GlobalVariable.Save();
            BaseGlobalVariable.Current.Finalize(application);
        }

        public static SessionGlobal NewDefaultSessionGlobal(HttpSessionState session)
        {
            SessionGlobal global = new SessionGlobal() { SessionId = session.SessionID };
            session[WebGlobalVariable.SESSION_DATA] = global;
            return global;
        }

        public static void SessionStart(HttpApplication application, SessionGlobal global)
        {
            WebGlobalVariable.WebCurrent.SessionStart(application, global);
        }

        public static void SessionEnd(HttpApplication application)
        {
            WebGlobalVariable.WebCurrent.SessionEnd(application);
        }
    }
}