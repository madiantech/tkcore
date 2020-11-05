using System;

namespace YJC.Toolkit.Sys
{
    internal class ApplicationConfigItem : BaseApplicationConfigItem
    {
        private const string DEFAULT_FILE = "Default.xml";
        private const string CONFIG_FILE = "Config.xml";
        private const string APP_RIGHT_BUILDER = "Empty";

        [SimpleAttribute]
        public string ActionResultName { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool UseWorkThread { get; private set; }

        [SimpleAttribute]
        public bool EnableCrossDomain { get; private set; }

        [SimpleAttribute(DefaultValue = "00:01:00")]
        public TimeSpan TimingInterval { get; private set; }

        [SimpleAttribute(DefaultValue = DEFAULT_FILE)]
        public string DefaultValueFile { get; private set; }

        [SimpleAttribute(DefaultValue = CONFIG_FILE)]
        public string ConfigFile { get; private set; }

        [SimpleAttribute(DefaultValue = APP_RIGHT_BUILDER)]
        public string AppRightBuilder { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public AppUrlConfigItem Url { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public AppInfoConfigItem Info { get; private set; }
    }
}