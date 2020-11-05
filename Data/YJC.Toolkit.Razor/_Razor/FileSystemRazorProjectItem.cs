using System.IO;
using System.Text.RegularExpressions;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class FileSystemRazorProjectItem : TkRazorProjectItem
    {
        internal static readonly Regex RegEx = new Regex(@"[^A-Za-z]*");

        public FileSystemRazorProjectItem(string templateKey, FileInfo fileInfo)
        {
            TkDebug.AssertArgumentNullOrEmpty(templateKey, nameof(templateKey), null);
            TkDebug.AssertArgumentNull(fileInfo, nameof(fileInfo), null);

            Key = templateKey;
            File = fileInfo;
        }

        public FileInfo File { get; }

        public override string Key { get; }

        public override bool Exists => File.Exists;

        public override string BaseAssemblyPath => "File";

        public override string CreateAssemblyName()
        {
            string fileName = Path.ChangeExtension(Key, null);
            fileName = RegEx.Replace(fileName, string.Empty);
            return $"fle_{fileName}_{File.LastWriteTime.Ticks}";
        }

        public override Stream Read() => File.OpenRead();
    }
}