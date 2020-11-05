using System;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [Serializable]
    public class RazorPath : IRazorPath
    {
        public RazorPath(string localPath)
            : this(localPath, null)
        {
        }

        public RazorPath(string localPath, string layoutFile)
        {
            TkDebug.AssertArgumentNullOrEmpty(localPath, "localPath", null);

            LocalPath = localPath;
            LayoutFile = layoutFile;
            if (!string.IsNullOrEmpty(LayoutFile))
                LayoutPath = Path.GetDirectoryName(LayoutFile);
        }

        #region IRazorPath 成员

        public string LocalPath { get; private set; }

        public string LayoutFile { get; private set; }

        public string LayoutPath { get; private set; }

        public void ClearLayoutFile()
        {
            LayoutFile = null;
        }

        #endregion

        public static explicit operator RazorPath(string path)
        {
            return new RazorPath(path);
        }

        public override string ToString()
        {
            return LocalPath;
        }
    }
}
