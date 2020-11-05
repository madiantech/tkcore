using System;
using System.Collections.Generic;
using System.Globalization;
using YJC.Toolkit.Properties;

namespace YJC.Toolkit.Sys
{
    public abstract class BaseAppSetting
    {
        private CultureInfo fCulture;
        internal const string DEFAULT_ACTION_NAME = "ActionResult";
        private readonly Dictionary<string, string> fHosts;

        public static BaseAppSetting Current { get; internal set; }

        protected BaseAppSetting()
        {
            ActionResultName = DEFAULT_ACTION_NAME;
            fHosts = new Dictionary<string, string>();
        }

        public string SolutionPath { get; protected internal set; }

        public string AppPath { get; protected internal set; }

        public string AppVirtualPath { get; set; }

        public string XmlPath { get; protected internal set; }

        public string PlugInPath { get; protected internal set; }

        public string ErrorPath { get; protected internal set; }

        public string ActionResultName { get; protected internal set; }

        public bool IsDebug { get; protected internal set; }

        public bool ShowException { get; protected internal set; }

        public bool UseCache { get; protected internal set; }

        public bool UseWorkThread { get; protected internal set; }

        public CultureInfo Culture
        {
            get
            {
                return fCulture;
            }
            protected internal set
            {
                fCulture = value;
                TkCore.Culture = value;
            }
        }

        public TimeSpan CacheTime { get; protected internal set; }

        public int CommandTimeout { get; protected internal set; }

        public ISecretKey SecretKey { get; protected internal set; }

        public ReadSettings ReadSettings { get; internal set; }

        public WriteSettings WriteSettings { get; internal set; }

        protected internal void AddHost(string name, string value)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);

            fHosts[name] = value;
        }

        public string VerfiyGetHostString(string name)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);

            string url;
            if (fHosts.TryGetValue(name, out url))
                return url;

            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                "没有在Application.xml中配置名称为{0}的Host", name), this);
            return null;
        }

        public Uri VerfiyGetHost(string name)
        {
            Uri result = GetHost(name);
            TkDebug.AssertNotNull(result, string.Format(ObjectUtil.SysCulture,
                "没有在Application.xml中配置名称为{0}的Host", name), this);

            return result;
        }

        public Uri GetHost(string name)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);

            string url;
            if (fHosts.TryGetValue(name, out url))
                return new Uri(url);

            return null;
        }

        public Uri GetHost(string name, Uri baseUri)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", this);
            TkDebug.AssertArgumentNull(baseUri, "baseUri", this);

            string url;
            if (fHosts.TryGetValue(name, out url))
            {
                return new Uri(baseUri, url);
            }
            return null;
        }
    }
}