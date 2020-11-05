using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Sys
{
    public sealed class ConsoleAppSetting : BaseAppSetting, ISupportDbContext
    {
        private const string CONNECTION_STRING_SECTION = "connectionStrings";
        private DbContextConfig fDefault;
        private readonly Dictionary<string, DbContextConfig> fDictionary;

        internal ConsoleAppSetting(string basePath, ConsoleAppXml xml)
        {
            fDictionary = new Dictionary<string, DbContextConfig>();

            SolutionPath = xml.Application.Path;
            if (!Path.IsPathRooted(SolutionPath))
                SolutionPath = Path.GetFullPath(Path.Combine(basePath, SolutionPath));
            AppPath = xml.Application.AppPath;
            if (string.IsNullOrEmpty(AppPath))
                AppPath = "bin/Debug/netcoreapp2.2";
            AppPath = Path.GetFullPath(Path.Combine(SolutionPath, AppPath));
            PlugInPath = xml.Application.PlugInPath;
            if (string.IsNullOrEmpty(PlugInPath))
                PlugInPath = "Modules";
            PlugInPath = Path.GetFullPath(Path.Combine(AppPath, PlugInPath));

            XmlPath = Path.Combine(SolutionPath, "Xml");
            ErrorPath = Path.Combine(SolutionPath, "Error");
            IsDebug = xml.Debug.Debug;
            ShowException = xml.Debug.ShowException;
            UseCache = xml.Application.UseCache;
            Culture = xml.Application.Culture;
            CacheTime = xml.Application.CacheTime;
            CommandTimeout = xml.Application.CommandTimeout;
            Single = xml.Application.Single;
            UseWorkThread = xml.Application.UseWorkThread;
            SecretKey = xml.SecretKey;

            ConfigDatabase(xml);

            if (xml.Hosts != null)
                foreach (var item in xml.Hosts)
                    AddHost(item.Key, item.Value);

            Current = this;
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

        public bool Single { get; private set; }

        private void ConfigDatabase(ConsoleAppXml xml)
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
                    //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    //var connectionStrings = config.ConnectionStrings.ConnectionStrings;
                    //var entities = from item in xml.Databases
                    //               where !string.IsNullOrEmpty(item.ProviderName)
                    //               select item;
                    //foreach (var item in entities)
                    //{
                    //    ConnectionStringSettings setting = new ConnectionStringSettings(item.Name,
                    //        item.ConnectionString, item.ProviderName);
                    //    connectionStrings.Remove(setting.Name);
                    //    connectionStrings.Add(setting);
                    //}
                    //config.Save(ConfigurationSaveMode.Modified);
                    //ConfigurationManager.RefreshSection(CONNECTION_STRING_SECTION);
                }
            }
        }
    }
}