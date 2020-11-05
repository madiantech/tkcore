using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Threading;

namespace YJC.Toolkit.Sys
{
    public abstract class BaseGlobalVariable : IDisposable
    {
        //private HashSet<string> fLoadedAssembly;
        //private Dictionary<string, Assembly> fLoadedAssemblyDict;
        //private Dictionary<string, string> fPlugIns;
        private readonly PlugInErrorLog fPlugInErrorLog;

        private AssemblyManager fManager;
        private PlugInAssembly fPlugIn;
        private ITrace fTrace;

        private static ThreadLocal<TkObjectContext> fObjectContext =
            new ThreadLocal<TkObjectContext>(() => new TkObjectContext());

        private WorkerThread fWorkerThread;

        public static BaseGlobalVariable Current { get; internal set; }

        protected BaseGlobalVariable()
        {
            FactoryManager = new PlugInFactoryManager();
            CacheManager = new CacheManager();
            fPlugInErrorLog = new PlugInErrorLog();
            DefaultValue = new ToolkitDefaultValue();
            Config = new DefaultToolkitConfig();
            InitializeToolkitCore();
        }

        protected bool NeitherContext { get; set; }

        public PlugInFactoryManager FactoryManager { get; private set; }

        public AppPathAssembly AppPathAssembly { get; private set; }

        public CacheManager CacheManager { get; private set; }

        public IDefaultValue DefaultValue { get; protected set; }

        public IConfig Config { get; protected set; }

        public abstract IUserInfo UserInfo { get; set; }

        public ITrace Trace
        {
            get
            {
                if (fTrace == null)
                    fTrace = DefaultTrace.Instance;
                return fTrace;
            }
            set
            {
                TkDebug.AssertArgumentNull(value, nameof(value), this);

                fTrace = value;
            }
        }

        public TkObjectContext ObjectContext => fObjectContext.Value;

        protected virtual AppPathAssembly CreateAppPathAssembly(string appPath, AssemblyManager manager)
        {
            //AppDomain domain = AppDomain.CurrentDomain;
            //Assembly[] assemblies = domain.GetAssemblies();

            AppPathAssembly result = new AppPathAssembly();
            //result.Add(ToolkitConst.TOOLKIT_CORE_NAME, ToolkitConst.TOOLKIT_CORE_ASSEMBLY);
            //foreach (var assembly in assemblies)
            //{
            //    try
            //    {
            //        string location = assembly.Location;
            //        if (!string.IsNullOrEmpty(location))
            //        {
            //            if (location.StartsWith(appPath, StringComparison.CurrentCultureIgnoreCase))
            //            {
            //                AssemblyName name = new AssemblyName(assembly.FullName);
            //                result.Add(name, assembly);
            //            }
            //            else
            //                result.AddLoadedAssembly(assembly);
            //        }
            //        else
            //            result.AddLoadedAssembly(assembly);
            //    }
            //    catch
            //    {
            //    }
            //}
            if (!Directory.Exists(appPath))
                return result;

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
                }
                catch (Exception ex)
                {
                    HandleStartedExeception("SLoadAssembly", GetType(), ex);
                }
            }

            return result;
        }

        protected Assembly CreateAssembly(AssemblyName assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName.CodeBase))
                return Assembly.Load(assemblyName);
            else
                return Assembly.LoadFile(assemblyName.CodeBase);
        }

        //private IEnumerable<AssemblyName> CreateModuleAssemblies(BaseAppSetting appSetting)
        //{
        //    IEnumerable<string> files = Directory.EnumerateFiles(appSetting.PlugInPath,
        //        "*.dll", SearchOption.AllDirectories);
        //    fPlugIns = new Dictionary<string, string>();
        //    foreach (var file in files)
        //    {
        //        AssemblyName name;
        //        try
        //        {
        //            name = AssemblyName.GetAssemblyName(file);
        //            if (string.IsNullOrEmpty(name.CodeBase))
        //                name.CodeBase = file;
        //            fPlugIns.Add(name.FullName, file);
        //        }
        //        catch
        //        {
        //            name = null;
        //        }
        //        if (name != null)
        //            yield return name;
        //    }
        //}

        private void InitializeToolkitCore()
        {
            AppPathAssembly.AddPlugInFactory(FactoryManager, ToolkitConst.TOOLKIT_CORE_ASSEMBLY);
            LoadAssembly(ToolkitConst.TOOLKIT_CORE_ASSEMBLY);
        }

        //protected IEnumerable<AssemblyName> GetModuleAssemblies(BaseAppSetting appSetting)
        //{
        //    if (!Directory.Exists(appSetting.PlugInPath))
        //        return Enumerable.Empty<AssemblyName>();
        //    else
        //    {
        //        return CreateModuleAssemblies(appSetting);
        //    }
        //}

        public void Initialize(BaseAppSetting appSetting, object application)
        {
            TkDebug.AssertArgumentNull(appSetting, "appSetting", this);

            fManager = new AssemblyManager();
            // 初始化AppPath下所有的Assembly
            AppPathAssembly = CreateAppPathAssembly(appSetting.AppPath, fManager);
            TkDebug.AssertNotNull(AppPathAssembly, "CreateAppPathAssembly返回为空", this);

            //fLoadedAssembly = AppPathAssembly.LoadedAssembly;
            // AppPath下的Assembly中查找添加PlugInFactory
            AppPathAssembly.AddPlugInFactory(FactoryManager);
            // 执行AppPath下的IInitialization
            var inits = AppPathAssembly.CreateInitializations();
            foreach (var item in inits)
            {
                try
                {
                    TkTrace.LogInfo($"执行{item.GetType()}的AppStarting");
                    item.AppStarting(application, appSetting, this);
                }
                catch (Exception ex)
                {
                    HandleStartedExeception("SAppStarting", item.GetType(), ex);
                }
            }

            // 搜索Code插件
            foreach (var assembly in AppPathAssembly)
            {
                if (assembly == ToolkitConst.TOOLKIT_CORE_ASSEMBLY)
                    continue;
                LoadAssembly(assembly);
            }

            //fLoadedAssemblyDict = new Dictionary<string, Assembly>();
            //Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //foreach (var ass in assemblies)
            //{
            //    if (!fLoadedAssemblyDict.ContainsKey(ass.FullName))
            //        fLoadedAssemblyDict.Add(ass.FullName, ass);
            //}

            if (NeitherContext)
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            fPlugIn = new PlugInAssembly(appSetting, fManager);
            //IEnumerable<AssemblyName> moduleAssembyNames = GetModuleAssemblies(appSetting);
            foreach (var assemblyName in fPlugIn)
            {
                if (!fManager.Contains(assemblyName.Name))
                {
                    var assembly = fPlugIn.LoadAssembly(fManager, NeitherContext, assemblyName);
                    //Assembly assembly = CreateAssembly(assemblyName);
                    //fLoadedAssemblyDict.Add(assembly.FullName, assembly);
                    if (assembly != null)
                    {
                        LoadAssembly(assembly);
                        //fLoadedAssembly.Add(assemblyName.FullName);
                    }
                }
            }

            if (NeitherContext)
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;

            // 搜索Xml插件
            var factories = FactoryManager.CodeFactories;
            foreach (var factory in factories)
                factory.SearchXmlPlugIn(appSetting.XmlPath);

            ProcessConfigDefaultValue(appSetting);
            foreach (var item in inits)
            {
                try
                {
                    TkTrace.LogInfo($"执行{item.GetType()}的AppStarted");
                    item.AppStarted(application, appSetting, this);
                }
                catch (Exception ex)
                {
                    HandleStartedExeception("SAppStarted", item.GetType(), ex);
                }
            }

            LogPlugError(fPlugInErrorLog);
        }

        private void ProcessConfigDefaultValue(BaseAppSetting appSetting)
        {
            ConfigTypeFactory typeFactory = FactoryManager.GetCodeFactory(
                ConfigTypeFactory.REG_NAME).Convert<ConfigTypeFactory>();
            Config.RegisterConfig(typeFactory);
            DefaultValueTypeFactory defaultFactory = FactoryManager.GetCodeFactory(
                DefaultValueTypeFactory.REG_NAME).Convert<DefaultValueTypeFactory>();
            DefaultValue.RegisterConfig(defaultFactory);

            ReadDefaultConfig(appSetting);
            ReadConfig(appSetting);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //if (fLoadedAssemblyDict.TryGetValue(args.Name, out Assembly result))
            //    return result;
            //if (fPlugIns.TryGetValue(args.Name, out string file))
            //{
            //    Assembly assembly = Assembly.LoadFrom(file);
            //    fLoadedAssemblyDict.Add(assembly.FullName, assembly);
            //    return assembly;
            //}
            var assembly = fManager.TryGetAssembly(args.Name);
            if (assembly != null)
                return assembly;
            return fPlugIn.TryLoadAssembly(fManager, NeitherContext, args.Name);
        }

        internal void AddCodeError(BasePlugInAttribute attribute, Type type, PlugInErrorType errorType)
        {
            fPlugInErrorLog.AddCodeError(attribute, type, errorType);
        }

        internal void AddXmlError(string regName, string xmlFile, PlugInErrorType errorType)
        {
            fPlugInErrorLog.AddXmlError(regName, xmlFile, errorType);
        }

        protected void LoadAssembly(Assembly assembly)
        {
            TkDebug.AssertArgumentNull(assembly, "assembly", this);

            Type currentType = null;
            try
            {
                TkTrace.LogInfo($"扫描{assembly.FullName}");
                Type[] types = assembly.GetTypes();

                Type attrType = typeof(BasePlugInAttribute);
                foreach (Type type in types)
                {
                    if (type.IsInterface)
                        continue;

                    currentType = type;
                    Attribute[] attrs = Attribute.GetCustomAttributes(type, attrType);
                    if (attrs.Length == 0)
                        continue;

                    foreach (BasePlugInAttribute attribute in attrs)
                        FactoryManager.InternalAddCodePlugIn(this, attribute, type);
                }

                TkTrace.LogInfo($"结束扫描{assembly.FullName}");
            }
            catch (Exception ex)
            {
                TkTrace.LogError($"加载{assembly.FullName}出错，错误：{ex.Message}");
                HandleStartedExeception("SLoadType", currentType, ex);
            }
        }

        protected virtual void LogPlugError(PlugInErrorLog errorLog)
        {
        }

        protected virtual void ReadDefaultConfig(BaseAppSetting appSetting)
        {
        }

        protected virtual void ReadConfig(BaseAppSetting appSetting)
        {
        }

        public virtual string ResolveUrl(string url)
        {
            return url;
        }

        public virtual void HandleStartedExeception(string startTag, Type errorType, Exception ex)
        {
        }

        public IAsyncResult BeginInvoke(Delegate method, params object[] args)
        {
            return BeginInvoke(null, method, args, null);
        }

        public IAsyncResult BeginInvoke(AsyncCallback callback, Delegate method,
            object[] args, object asyncState)
        {
            if (fWorkerThread != null)
                return fWorkerThread.BeginInvoke(callback, method, args, asyncState);

            return null;
        }

        public object EndInvoke(IAsyncResult result)
        {
            if (fWorkerThread != null)
                return fWorkerThread.EndInvoke(result);
            return null;
        }

        public ITkTypeConverter GetExTypeConverter(Type type)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            Type converterType = converter.GetType();
            if (converterType != typeof(ReferenceConverter) && converterType != typeof(TypeConverter))
                return new WrapTypeConverter(converter);
            return null;
        }

        public ITkTypeConverter GetExTypeConverter(PropertyInfo propertyInfo)
        {
            Attribute attr = Attribute.GetCustomAttribute(propertyInfo, typeof(TypeConverterAttribute));
            if (attr != null)
            {
                string typeName = attr.Convert<TypeConverterAttribute>().ConverterTypeName;
                Type type = Type.GetType(typeName);
                if (type != null)
                {
                    TypeConverter converter = ObjectUtil.CreateObject(type).Convert<TypeConverter>();
                    return new WrapTypeConverter(converter);
                }
            }

            return null;
        }

        public void Finalize(object application)
        {
            var inits = AppPathAssembly.CreateInitializations();
            foreach (var item in inits)
                try
                {
                    item.AppEnd(application);
                }
                catch (Exception ex)
                {
                    HandleStartedExeception("EAppEnd", item.GetType(), ex);
                }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal void CreateWorkThread()
        {
            fWorkerThread = new WorkerThread(true);
        }

        internal void CloseWorkThread()
        {
            fWorkerThread.DisposeObject();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (fWorkerThread != null)
                    fWorkerThread.Dispose();
                if (fObjectContext != null)
                {
                    fObjectContext.Dispose();
                    fObjectContext = null;
                }
            }
        }

        public static object UserId
        {
            get
            {
                TkDebug.ThrowIfNoGlobalVariable();
                return Current.UserInfo == null ? null : Current.UserInfo.UserId;
            }
        }

        public static string UserName
        {
            get
            {
                TkDebug.ThrowIfNoGlobalVariable();
                return Current.UserInfo == null ? null : Current.UserInfo.UserName;
            }
        }
    }
}