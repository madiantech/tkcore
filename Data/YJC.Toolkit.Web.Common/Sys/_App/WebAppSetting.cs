using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public sealed class WebAppSetting : BaseAppSetting, ISupportDbContext
    {
        private class TempPageMakerConfig : IConfigCreator<IPageMaker>
        {
            private readonly IPageMaker fPageMaker;

            public TempPageMakerConfig(IPageMaker pageMaker)
            {
                fPageMaker = pageMaker;
            }

            #region IConfigCreator<IPageMaker> 成员

            public IPageMaker CreateObject(params object[] args)
            {
                return fPageMaker;
            }

            #endregion IConfigCreator<IPageMaker> 成员
        }

        private static WebAppSetting fCurrent;

        private DbContextConfig fDefault;
        private readonly Dictionary<string, DbContextConfig> fDictionary;
        private readonly string fQueryStringName;
        private readonly string fQueryStringValue;
        private readonly string fMetaDataValue;
        private readonly string fJsonValue;
        private readonly string fExcelValue;
        private readonly string fUploadTempPath;
        private ExceptionIndexer fExceptionIndexer;

        //private readonly string fUploadPath;
        private string fUploadTempVirtualPath;

        //private string fUploadVirtualPath;

        internal WebAppSetting(string basePath, WebAppXml xml)
        {
            fDictionary = new Dictionary<string, DbContextConfig>();

            SolutionPath = xml.Application.Path;
            if (!Path.IsPathRooted(SolutionPath))
                SolutionPath = Path.GetFullPath(Path.Combine(basePath, SolutionPath));
            AppPath = xml.Application.AppPath;
            if (string.IsNullOrEmpty(AppPath))
                AppPath = "web/bin";
            AppPath = Path.GetFullPath(Path.Combine(SolutionPath, AppPath));
            PlugInPath = xml.Application.PlugInPath;
            if (string.IsNullOrEmpty(PlugInPath))
                PlugInPath = "Modules";
            PlugInPath = Path.GetFullPath(Path.Combine(AppPath, PlugInPath));
            if (!string.IsNullOrEmpty(xml.Application.ActionResultName))
                ActionResultName = xml.Application.ActionResultName;
            EnableCrossDomain = xml.Application.EnableCrossDomain;

            XmlPath = Path.Combine(SolutionPath, "Xml");
            ErrorPath = Path.Combine(SolutionPath, "Error");
            IsDebug = xml.Debug.Debug;
            ShowException = xml.Debug.ShowException;
            fQueryStringName = xml.Debug.XmlQueryString;
            fQueryStringValue = xml.Debug.XmlValue;
            fMetaDataValue = xml.Debug.MetaDataValue;
            fJsonValue = xml.Debug.JsonValue;
            fExcelValue = xml.Debug.ExcelValue;
            UseCache = xml.Application.UseCache;
            Culture = xml.Application.Culture;
            UseWorkThread = xml.Application.UseWorkThread;
            CacheTime = xml.Application.CacheTime;
            TimingInterval = xml.Application.TimingInterval;
            CommandTimeout = xml.Application.CommandTimeout;
            DefaultValueFile = xml.Application.DefaultValueFile;
            ConfigFile = xml.Application.ConfigFile;
            AppRightBuilder = xml.Application.AppRightBuilder;
            SecretKey = xml.SecretKey;
            StartupPath = xml.Application.Url.StartupPath;
            //string resolveStartPath = WebUtil.ResolveUrl(StartupPath);
            //LogOnPath = WebUtil.ResolveUrl(xml.Application.Url.LogOnPath);
            //if (string.IsNullOrEmpty(LogOnPath))
            //    LogOnPath = resolveStartPath;
            //HomePath = WebUtil.ResolveUrl(xml.Application.Url.HomePath);
            //if (string.IsNullOrEmpty(HomePath))
            //    HomePath = resolveStartPath;
            //MainPath = WebUtil.ResolveUrl(xml.Application.Url.MainPath);
            string errorPage = xml.Application.Url.ErrorPage;
            if (!string.IsNullOrEmpty(errorPage))
            {
                ErrorPagePath = Path.GetFullPath(Path.Combine(AppPath, "..", errorPage));
                ErrorPageUri = new Uri(errorPage, UriKind.Relative);
            }
            AppInfoConfigItem appInfo = xml.Application.Info;
            if (appInfo != null)
            {
                AppFullName = appInfo.FullName.ToString();
                AppShortName = appInfo.ShortName.ToString();
                AppDescription = appInfo.Description == null ? string.Empty : appInfo.Description.ToString();
            }
            else
                AppFullName = AppShortName = AppDescription = string.Empty;

            if (xml.IO != null)
            {
                InputGZip = xml.IO.InputGZip;
                InputEncrypt = xml.IO.InputEncrypt;
                OutputGZip = xml.IO.OutputGZip;
                OutputEncrypt = xml.IO.OutputEncrypt;
            }
            if (xml.Upload != null)
            {
                string baseUploadPath = SolutionPath;
                fUploadTempPath = Path.Combine(baseUploadPath, xml.Upload.TempPath);
                //fUploadPath = Path.Combine(baseUploadPath, xml.Upload.Path);
                //if (!Directory.Exists(fUploadPath))
                //    Directory.CreateDirectory(fUploadPath);
                if (!Directory.Exists(fUploadTempPath))
                    Directory.CreateDirectory(fUploadTempPath);
                //fUploadVirtualPath = xml.Upload.VirtualPath;
                //if (!string.IsNullOrEmpty(fUploadVirtualPath))
                //    fUploadVirtualPath = VirtualPathUtility.AppendTrailingSlash(fUploadVirtualPath);
                fUploadTempVirtualPath = xml.Upload.TempVirtualPath;
                //if (!string.IsNullOrEmpty(fUploadTempVirtualPath))
                //    fUploadTempVirtualPath = VirtualPathUtility.AppendTrailingSlash(fUploadTempVirtualPath);
            }
            ConfigDatabase(xml);

            Current = this;
            fCurrent = this;

            if (xml.Hosts != null)
                foreach (var item in xml.Hosts)
                    AddHost(item.Key, item.Value);

            InitialIndexer();

            // 假处理，以后需要考虑解决方案
            //AppVirtualPath = "/";
            //AppVirtualPath = HttpRuntime.AppDomainAppVirtualPath;
            //if (string.IsNullOrEmpty(AppVirtualPath))
            //    AppVirtualPath = "/";
            //AppVirtualPath = VirtualPathUtility.AppendTrailingSlash(AppVirtualPath);
        }

        #region ISupportDbContext 成员

        public DbContextConfig Default
        {
            get
            {
                TkDebug.AssertNotNull(fDefault, "没有配置默认的数据库连接", this);
                return fDefault;
            }
        }

        public DbContextConfig GetContextConfig(string name)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);
            //TkDebug.AssertArgument(fDictionary.ContainsKey(name), "name",
            //    string.Format(ObjectUtil.SysCulture, "配置中没有名称为{0}的数据库连接", name), this);
            DbContextConfig result;
            if (fDictionary.TryGetValue(name, out result))
                return result;
            return Default;
        }

        #endregion ISupportDbContext 成员

        public static WebAppSetting WebCurrent
        {
            get
            {
                return fCurrent;
            }
        }

        public string UploadTempPath
        {
            get
            {
                TkDebug.AssertNotNullOrEmpty(fUploadTempPath,
                    "Application.xml中没有配置Upload节点", this);
                return fUploadTempPath;
            }
        }

        public string UploadTempVirtualPath
        {
            get
            {
                TkDebug.AssertNotNullOrEmpty(fUploadTempVirtualPath,
                    "Application.xml中没有配置Upload节点", this);
                return fUploadTempVirtualPath;
            }
            internal set
            {
                fUploadTempVirtualPath = value;
            }
        }

        //public string UploadPath
        //{
        //    get
        //    {
        //        TkDebug.AssertNotNullOrEmpty(fUploadTempPath,
        //            "Application.xml中没有配置Upload节点", this);
        //        return fUploadPath;
        //    }
        //}

        //public string UploadVirtualPath
        //{
        //    get
        //    {
        //        TkDebug.AssertNotNullOrEmpty(fUploadTempPath,
        //            "Application.xml中没有配置Upload节点", this);
        //        return fUploadVirtualPath;
        //    }
        //    internal set
        //    {
        //        fUploadVirtualPath = value;
        //    }
        //}

        public bool InputGZip { get; private set; }

        public bool InputEncrypt { get; private set; }

        public bool OutputGZip { get; private set; }

        public bool OutputEncrypt { get; private set; }

        public string StartupPath { get; private set; }

        public string LogOnPath { get; internal set; }

        public Uri ErrorPageUri { get; private set; }

        public TimeSpan TimingInterval { get; private set; }

        public string ErrorPagePath { get; private set; }

        public string HomePath { get; internal set; }

        public string MainPath { get; internal set; }

        public string AppFullName { get; private set; }

        public string AppShortName { get; private set; }

        public string AppDescription { get; private set; }

        public bool EnableCrossDomain { get; private set; }

        public string DefaultValueFile { get; private set; }

        public string ConfigFile { get; private set; }

        public string AppRightBuilder { get; private set; }

        public bool IsDevelopment { get; internal set; }

        public IConfigCreator<IPageMaker> DefaultPageMaker { get; internal set; }

        public IConfigCreator<IRedirector> DefaultRedirector { get; internal set; }

        public IConfigCreator<IPostObjectCreator> DefaultPostCreator { get; internal set; }

        //public IConfigCreator<IExceptionHandler> ExceptionHandler { get; private set; }

        //public IConfigCreator<IExceptionHandler> ReLogOnHandler { get; private set; }

        //public IConfigCreator<IExceptionHandler> ErrorPageHandler { get; private set; }

        //public IConfigCreator<IExceptionHandler> ToolkitHandler { get; private set; }

        //public IConfigCreator<IExceptionHandler> ErrorOpeartionHandler { get; private set; }

        private void ConfigDatabase(WebAppXml xml)
        {
            if (xml.Databases != null)
            {
                bool entity = false;
                foreach (var item in xml.Databases)
                {
                    if (item.Default)
                    {
                        TkDebug.Assert(fDefault == null, string.Format(ObjectUtil.SysCulture,
                            "{0}的Default属性标识为True，而前面已经有配置Default为True了，配置错误", item.Name), this);
                        fDefault = item;
                    }
                    if (!string.IsNullOrEmpty(item.ProviderName))
                        entity = true;
                    fDictionary.Add(item.Name, item);
                }
                if (fDefault == null && xml.Databases.Count > 0)
                    fDefault = xml.Databases[0];

                if (entity)
                {
                    //Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpRuntime.AppDomainAppVirtualPath);
                    //var connectionStrings = config.ConnectionStrings.ConnectionStrings;
                    //bool isModify = false;
                    //var entities = from item in xml.Databases
                    //               where !string.IsNullOrEmpty(item.ProviderName)
                    //               select item;
                    //foreach (var item in entities)
                    //{
                    //    ConnectionStringSettings setting = new ConnectionStringSettings(item.Name,
                    //        item.ConnectionString, item.ProviderName);
                    //    int index = connectionStrings.IndexOf(setting);
                    //    if (index == -1)
                    //    {
                    //        isModify = true;
                    //        connectionStrings.Remove(setting.Name);
                    //        connectionStrings.Add(setting);
                    //    }
                    //}
                    //if (isModify)
                    //    config.Save(ConfigurationSaveMode.Modified);
                }
            }
        }

        //internal void Config(WebAppExtensionXml extXml)
        //{
        //    DefaultConfigItem defaultConfig = extXml.DefaultConfig;
        //    if (defaultConfig != null)
        //    {
        //        DefaultPageMaker = defaultConfig.DefaultPageMaker;
        //        DefaultRedirector = defaultConfig.DefaultRedirector;
        //        DefaultPostCreator = defaultConfig.DefaultPostObjectCreator;
        //        ReadSettings = defaultConfig.ReadSettings;
        //        WriteSettings = defaultConfig.WriteSettings;
        //    }

        // if (DefaultPageMaker == null) DefaultPageMaker = new SourceOutputPageMakerConfig(); if
        // (DefaultRedirector == null) DefaultRedirector = new OutputRedirectorConfig(); if
        // (DefaultPostCreator == null) DefaultPostCreator = new JsonPostDataSetCreatorConfig(); if
        // (ReadSettings == null) ReadSettings = ReadSettings.Default; if (WriteSettings == null)
        // WriteSettings = WriteSettings.Default;

        // ExceptionHandlerConfigItem exConfig = extXml.ExceptionHandler; if (exConfig != null) {
        // ErrorPageHandler = exConfig.ErrorPageException; ErrorOpeartionHandler =
        // exConfig.ErrorOperationException; ReLogOnHandler = exConfig.ReLogonException;
        // ToolkitHandler = exConfig.ToolkitException; ExceptionHandler = exConfig.Exception; }

        //    var tempHandleConfig = new PageMakerExceptionHandlerConfig()
        //    {
        //        PageMaker = new TempPageMakerConfig(ExceptionPageMaker.Instance)
        //    };
        //    if (ErrorPageHandler == null)
        //        ErrorPageHandler = tempHandleConfig;
        //    if (ErrorOpeartionHandler == null)
        //    {
        //        ErrorOpeartionHandler = tempHandleConfig;
        //    }
        //    if (ReLogOnHandler == null)
        //        ReLogOnHandler = new ReLogonExceptionHandlerConfig();
        //    if (ToolkitHandler == null)
        //        ToolkitHandler = tempHandleConfig;
        //    if (ExceptionHandler == null)
        //        ExceptionHandler = new PageMakerExceptionHandlerConfig()
        //        {
        //            Log = true,
        //            PageMaker = new TempPageMakerConfig(ExceptionPageMaker.Instance)
        //        };
        //}

        public bool IsShowSource(IQueryString queryString)
        {
            TkDebug.AssertArgumentNull(queryString, "queryString", this);

            return queryString[fQueryStringName] == fQueryStringValue;
        }

        public bool IsShowMetaData(IQueryString queryString)
        {
            TkDebug.AssertArgumentNull(queryString, "queryString", this);

            return queryString[fQueryStringName] == fMetaDataValue;
        }

        public bool IsShowJson(IQueryString queryString)
        {
            TkDebug.AssertArgumentNull(queryString, "queryString", this);

            return queryString[fQueryStringName] == fJsonValue;
        }

        public bool IsShowExcel(IQueryString queryString)
        {
            TkDebug.AssertArgumentNull(queryString, "queryString", this);

            return queryString[fQueryStringName] == fExcelValue;
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

        internal string GetStartLogName(string startName, Exception ex)
        {
            string fileName = string.Format(ObjectUtil.SysCulture, "{2}_{0}_{1}.xml",
                ex.GetType().Name, fExceptionIndexer.NextIndex(), startName);
            return Path.Combine(BaseAppSetting.Current.ErrorPath, fileName);
        }
    }
}