using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.SessionState;

namespace YJC.Toolkit.Sys
{
    [EvaluateAddition(Author = "YJC", CreateDate = "2018-04-16",
        Description = "添加WebGlobalVariable类型扩展")]
    public sealed class WebGlobalVariable : BaseGlobalVariable
    {
        public static readonly string SESSION_DATA = "_GlobalSession";
        private static WebGlobalVariable fCurrent;
        private ExceptionIndexer fExceptionIndexer;

        internal WebGlobalVariable()
        {
            fCurrent = this;
            Current = this;
        }

        public override IUserInfo UserInfo
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return SessionGbl.Info;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                TkDebug.AssertArgumentNull(value, "value", this);

                SessionGbl.Info = value;
            }
        }

        internal void SessionStart(HttpApplication application, SessionGlobal sessionGlobal)
        {
            var inits = AppPathAssembly.CreateInitializations();
            foreach (var item in inits)
            {
                IWebInitialization webInit = item as IWebInitialization;
                if (webInit != null)
                    try
                    {
                        webInit.SessionStart(application, sessionGlobal);
                    }
                    catch (Exception ex)
                    {
                        HandleStartedExeception("SSessionStart", webInit.GetType(), ex);
                    }
            }
        }

        internal void SessionEnd(HttpApplication application)
        {
            var inits = AppPathAssembly.CreateInitializations();
            foreach (var item in inits)
            {
                IWebInitialization webInit = item as IWebInitialization;
                if (webInit != null)
                    try
                    {
                        webInit.SessionEnd(application);
                    }
                    catch (Exception ex)
                    {
                        HandleStartedExeception("SSessionEnd", webInit.GetType(), ex);
                    }
            }
        }

        protected override AppPathAssembly CreateAppPathAssembly(string appPath, AssemblyManager manager)
        {
            AppPathAssembly result = new AppPathAssembly();
            if (!Directory.Exists(appPath))
                return result;

            //AppDomain domain = AppDomain.CurrentDomain;
            //Assembly[] assemblies = domain.GetAssemblies();

            //Dictionary<string, Assembly> loaded = new Dictionary<string, Assembly>();
            //foreach (var assembly in assemblies)
            //    loaded.Add(assembly.FullName, assembly);

            IEnumerable<string> binFiles = Directory.EnumerateFiles(appPath, "*.dll", SearchOption.TopDirectoryOnly);
            foreach (string fileName in binFiles)
            {
                try
                {
                    AssemblyName name = AssemblyName.GetAssemblyName(fileName);
                    if (!result.Constains(name))
                    {
                        Assembly assembly = manager.TryGetAssembly(name.FullName);
                        if (assembly == null)
                            assembly = manager.LoadAssembly(name);
                        result.Add(name, assembly);
                    }
                    // AssemblyName name = AssemblyName.GetAssemblyName(fileName); Assembly assembly;
                    // if (loaded.TryGetValue(name.FullName, out assembly)) { result.Add(name,
                    // assembly); loaded.Remove(name.FullName); } else { assembly =
                    // Assembly.Load(name); result.Add(name, assembly); }
                }
                catch (Exception ex)
                {
                    HandleStartedExeception("SLoadAssembly", GetType(), ex);
                }
            }
            //foreach (var item in loaded.Values)
            //    result.AddLoadedAssembly(item);

            return result;
        }

        internal void InitialIndexer()
        {
            fExceptionIndexer = new ExceptionIndexer();
        }

        internal string GetExceptionLogName(Exception ex)
        {
            string fileName = string.Format(ObjectUtil.SysCulture, "{0}_{1}.xml",
                ex.GetType().Name, fExceptionIndexer.NextIndex());
            return Path.Combine(BaseAppSetting.Current.ErrorPath, fileName);
        }

        protected override void ReadDefaultConfig(BaseAppSetting appSetting)
        {
            WebAppSetting webSetting = appSetting.Convert<WebAppSetting>();
            string fileName = Path.Combine(appSetting.XmlPath, webSetting.DefaultValueFile);

            WebDefaultValue = new WebDefaultXmlConfig();
            if (File.Exists(fileName))
                WebDefaultValue.ReadXmlFromFile(fileName);
            else
                WebDefaultValue.OnReadObject();
            DefaultValue = WebDefaultValue;
        }

        protected override void ReadConfig(BaseAppSetting appSetting)
        {
            WebAppSetting webSetting = appSetting.Convert<WebAppSetting>();
            string fileName = Path.Combine(appSetting.XmlPath, webSetting.ConfigFile);

            if (File.Exists(fileName))
                Config.ReadXmlFromFile(fileName);
        }

        protected override void LogPlugError(PlugInErrorLog errorLog)
        {
            if (errorLog.HasError)
            {
                string fileName = Path.Combine(BaseAppSetting.Current.XmlPath, "Dup.xml");
                string content = errorLog.WriteXml(WriteSettings.Default, QName.ToolkitNoNS);
                FileUtil.SaveFile(fileName, content, WriteSettings.Default.Encoding);
            }
        }

        public override void HandleStartedExeception(string startTag, Type errorType, Exception ex)
        {
            ExceptionUtil.HandleStartExecption(startTag, errorType, ex);
        }

        internal string GetStartLogName(string startName, Exception ex)
        {
            string fileName = string.Format(ObjectUtil.SysCulture, "{2}_{0}_{1}.xml",
                ex.GetType().Name, fExceptionIndexer.NextIndex(), startName);
            return Path.Combine(BaseAppSetting.Current.ErrorPath, fileName);
        }

        internal static void AssertHttpContext()
        {
            TkDebug.AssertNotNull(HttpContext.Current,
                "当前的HttpContext不存在，不能使用与Web相关的对象", null);
        }

        internal WebDefaultXmlConfig WebDefaultValue { get; private set; }

        public static HttpApplicationState Application
        {
            get
            {
                AssertHttpContext();
                return HttpContext.Current.Application;
            }
        }

        public static HttpSessionState Session
        {
            get
            {
                AssertHttpContext();
                return HttpContext.Current.Session;
            }
        }

        public static HttpServerUtility Server
        {
            get
            {
                AssertHttpContext();
                return HttpContext.Current.Server;
            }
        }

        public static HttpRequest Request
        {
            get
            {
                AssertHttpContext();
                return HttpContext.Current.Request;
            }
        }

        public static HttpResponse Response
        {
            get
            {
                AssertHttpContext();
                return HttpContext.Current.Response;
            }
        }

        public static SessionGlobal SessionGbl
        {
            get
            {
                SessionGlobal sessionGbl = Session[SESSION_DATA].Convert<SessionGlobal>();
                return sessionGbl;
            }
        }

        public static WebGlobalVariable WebCurrent
        {
            get
            {
                return fCurrent;
            }
        }

        public static IUserInfo Info
        {
            get
            {
                return SessionGbl.Info;
            }
        }
    }
}