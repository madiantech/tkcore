using System;
using System.IO;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    public static class TestApp
    {
        public static void Initialize(string solutionPath)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            var setting = new TestAppSetting(solutionPath, Path.GetDirectoryName(assembly.Location));
            var global = new TestGlobalVariable();
            global.Initialize(setting, null);
        }
    }
}