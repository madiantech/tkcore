using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Sys
{
    internal sealed class ToolAppSetting : BaseAppSetting, ISupportDbContext
    {
        private DbContextConfig fDefault;
        private readonly Dictionary<string, DbContextConfig> fDictionary;

        public ToolAppSetting(string solutionPath, string appPath)
        {
            fDictionary = new Dictionary<string, DbContextConfig>();

            SolutionPath = solutionPath;
            XmlPath = Path.Combine(SolutionPath, "Xml");
            ErrorPath = Path.Combine(SolutionPath, "Error");
            AppPath = appPath;
            IsDebug = true;
            UseCache = true;
            Culture = Thread.CurrentThread.CurrentCulture;
            CacheTime = new TimeSpan(0, 20, 0);
            ShowException = true;
            CommandTimeout = 600;
            Current = this;
            ReadSettings = ReadSettings.Default;
            WriteSettings = WriteSettings.Default;
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

            DbContextConfig result;
            if (fDictionary.TryGetValue(name, out result))
                return result;
            return Default;
        }

        #endregion

        public void AddContextConfig(DbContextConfig config)
        {
            TkDebug.AssertArgumentNull(config, "config", this);

            if (fDefault == null)
                fDefault = config;

            if (!string.IsNullOrEmpty(config.Name))
                fDictionary.Add(config.Name, config);
        }

        public void AddBaseHost(string host, string value)
        {
            AddHost(host, value);
        }
    }
}
