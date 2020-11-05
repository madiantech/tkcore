using System.IO;

namespace YJC.Toolkit.Razor
{
    internal class ToolkitRazorProjectItem : FileSystemRazorProjectItem
    {
        public ToolkitRazorProjectItem(string templateKey, FileInfo fileInfo)
            : base(templateKey, fileInfo)
        {
        }

        public override string BaseAssemblyPath => "Toolkit";

        public override string CreateAssemblyName()
        {
            string fileName = Path.ChangeExtension(Key, null);
            fileName = RegEx.Replace(fileName, string.Empty);
            return $"tk_{fileName}_{File.LastWriteTime.Ticks}";
        }
    }
}