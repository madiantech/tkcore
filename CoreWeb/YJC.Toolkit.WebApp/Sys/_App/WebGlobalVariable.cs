using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    [EvaluateAddition(Author = "YJC", CreateDate = "2018-04-16",
        Description = "添加WebGlobalVariable类型扩展")]
    public sealed class WebGlobalVariable : BaseGlobalVariable
    {
        public static readonly string SESSION_DATA = "_GlobalSession";
        public static readonly IUserInfo EmptyInfo = new SimpleUserInfo();
        private static WebGlobalVariable fCurrent;
        //private ExceptionIndexer fExceptionIndexer;

        internal WebGlobalVariable()
        {
            fCurrent = this;
            Current = this;
            NeitherContext = true;
            WebDefaultValue = new WebDefaultXmlConfig();
            DefaultValue = WebDefaultValue;
        }

        public override IUserInfo UserInfo
        {
            get
            {
                if (HttpContextHelper.Current == null)
                    return null;
                var principal = Context.User;
                if (principal is ToolkitClaimsPrincipal tkPrincipal)
                    return tkPrincipal.UserInfo;
                return EmptyInfo;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                TkDebug.AssertArgumentNull(value, "value", this);

                if (value is JWTUserInfo userInfo)
                    Context.User = new ToolkitClaimsPrincipal(userInfo);
                //SessionGbl.Info = value;
            }
        }

        internal List<TimingJob> Jobs { get; } = new List<TimingJob>();

        internal WebDefaultXmlConfig WebDefaultValue { get; private set; }

        protected override AppPathAssembly CreateAppPathAssembly(string appPath, AssemblyManager manager)
        {
            AppPathAssembly result = new AppPathAssembly();
            if (!Directory.Exists(appPath))
                return result;

            //AppDomain domain = AppDomain.CurrentDomain;
            //Assembly[] assemblies = domain.GetAssemblies();

            //Dictionary<string, Assembly> loaded = new Dictionary<string, Assembly>();
            //foreach (var assembly in assemblies)
            //    if (!loaded.ContainsKey(assembly.FullName))
            //        loaded.Add(assembly.FullName, assembly);

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
                    //if (loaded.TryGetValue(name.FullName, out assembly))
                    //{
                    //    result.Add(name, assembly);
                    //    loaded.Remove(name.FullName);
                    //}
                    //else
                    //{
                    //    assembly = Assembly.Load(name);
                    //    result.Add(name, assembly);
                    //}
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

        //internal void InitialIndexer()
        //{
        //    fExceptionIndexer = new ExceptionIndexer();
        //}

        //internal string GetExceptionLogName(Exception ex)
        //{
        //    string fileName = string.Format(ObjectUtil.SysCulture, "{0}_{1}.xml",
        //        ex.GetType().Name, fExceptionIndexer.NextIndex());
        //    return Path.Combine(BaseAppSetting.Current.ErrorPath, fileName);
        //}

        protected override void ReadDefaultConfig(BaseAppSetting appSetting)
        {
            WebAppSetting webSetting = appSetting.Convert<WebAppSetting>();
            string fileName = Path.Combine(appSetting.XmlPath, webSetting.DefaultValueFile);

            if (File.Exists(fileName))
                WebDefaultValue.ReadXmlFromFile(fileName);
            else
                WebDefaultValue.OnReadObject();
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
            TimingJobPlugInFactory factory = FactoryManager.GetCodeFactory(
                TimingJobPlugInFactory.REG_NAME).Convert<TimingJobPlugInFactory>();
            factory.FillTimingJobs(Jobs);
        }

        public override void HandleStartedExeception(string startTag, Type errorType, Exception ex)
        {
            ExceptionUtil.HandleStartExecption(startTag, errorType, ex);
        }

        internal static void AssertHttpContext()
        {
            TkDebug.AssertNotNull(HttpContextHelper.Current,
                "当前的HttpContext不存在，不能使用与Web相关的对象", null);
        }

        public override string ResolveUrl(string url)
        {
            return WebUtil.ResolveUrl(url);
            //return null;
        }

        public static ISession Session
        {
            get
            {
                AssertHttpContext();
                return Context.Session;
            }
        }

        public static HttpContext Context
        {
            get
            {
                AssertHttpContext();
                return HttpContextHelper.Current;
            }
        }

        public static HttpRequest Request
        {
            get
            {
                AssertHttpContext();
                return HttpContextHelper.Current.Request;
            }
        }

        public static HttpResponse Response
        {
            get
            {
                AssertHttpContext();
                return HttpContextHelper.Current.Response;
            }
        }

        public static SessionGlobal SessionGbl
        {
            get
            {
                SessionGlobal sessionGbl = SessionGlobal.GetSessionGlobal(Info);
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
                return WebCurrent.UserInfo;
            }
        }
    }
}