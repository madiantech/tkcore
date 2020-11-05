namespace YJC.Toolkit.Sys
{
    public static class ConfigUtil
    {
        private static object InternalReadConfig(string sectionName)
        {
            TkDebug.ThrowIfNoGlobalVariable();

            var result = BaseGlobalVariable.Current.Config.GetConfig(sectionName);
            return result;
        }

        public static object ReadConfig(string sectionName)
        {
            TkDebug.AssertArgumentNullOrEmpty(sectionName, nameof(sectionName), null);

            var config = InternalReadConfig(sectionName);
            TkDebug.AssertNotNull(config, $"Config.xml中没有{sectionName}的配置，请确认", null);

            return config;
        }
    }
}