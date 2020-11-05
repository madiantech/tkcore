using System;
using System.IO;
using System.Reflection;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Sys
{
    public static class ToolApp
    {
        public static void Initialize(bool useWorkThread)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            string startPath = Path.GetDirectoryName(assembly.Location);
            var setting = new ToolAppSetting(startPath, startPath);
            var global = new ToolGlobalVariable();
            global.Initialize(setting, null);
            if (useWorkThread)
                global.CreateWorkThread();
        }

        public static void FinalizeApp()
        {
            var global = BaseGlobalVariable.Current.Convert<ToolGlobalVariable>();
            global.CloseWorkThread();
            BaseGlobalVariable.Current.Finalize(null);
        }

        public static void AddHost(string name, string value)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", null);
            TkDebug.AssertArgumentNullOrEmpty(value, "value", null);

            ((ToolAppSetting)BaseAppSetting.Current).AddBaseHost(name, value);
        }

        public static void AddContextConfig(DbContextConfig config)
        {
            ((ToolAppSetting)BaseAppSetting.Current).AddContextConfig(config);
        }
    }
}