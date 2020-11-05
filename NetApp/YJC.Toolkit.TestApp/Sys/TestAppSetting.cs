using System;
using System.IO;
using System.Threading;

namespace YJC.Toolkit.Sys
{
    internal sealed class TestAppSetting : BaseAppSetting
    {
        public TestAppSetting(string solutionPath, string appPath)
        {
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
    }
}
