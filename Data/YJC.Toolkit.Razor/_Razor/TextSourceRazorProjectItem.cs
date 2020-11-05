using System.IO;
using System.Reflection;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class TextSourceRazorProjectItem : TkRazorProjectItem
    {
        private readonly string fContent;

        public TextSourceRazorProjectItem(string key, string content)
        {
            TkDebug.AssertArgumentNullOrEmpty(key, nameof(key), null);
            TkDebug.AssertArgumentNullOrEmpty(content, nameof(content), null);

            Key = key;
            fContent = content;
        }

        public override string Key { get; }

        public override bool Exists => true;

        public string Content => fContent;

        public override string BaseAssemblyPath => "Text";

        public override string CreateAssemblyName()
        {
            return $"txt_{Key}_{fContent.GetHashCode()}";
        }

        public override Stream Read()
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(fContent));
        }

        //private string GetPath()
        //{
        //    string result = Path.Combine(BaseAppSetting.Current.XmlPath, @"Razor\temp", "Text");
        //    if (!Directory.Exists(result))
        //        Directory.CreateDirectory(result);
        //    return result;
        //}

        //private void SaveStream(string fileName, Stream stream)
        //{
        //    FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Read);
        //    using (fileStream)
        //    {
        //        FileUtil.CopyStream(stream, fileStream);
        //    }
        //}

        //public override void SaveAssembly(Stream assembly, Stream pdb)
        //{
        //    string path = GetPath();
        //    assembly.Seek(0, SeekOrigin.Begin);
        //    SaveStream(Path.Combine(path, CreateAssemblyName() + ".dll"), assembly);
        //    pdb.Seek(0, SeekOrigin.Begin);
        //    SaveStream(Path.Combine(path, CreateAssemblyName() + ".pdb"), pdb);
        //}

        //public override Assembly TryLoadAssembly()
        //{
        //    string path = GetPath();
        //    string assemblyName = Path.Combine(path, CreateAssemblyName() + ".dll");
        //    if (File.Exists(assemblyName))
        //    {
        //        byte[] assemblyDatas = File.ReadAllBytes(assemblyName);
        //        string pdbName = Path.Combine(path, CreateAssemblyName() + ".pdb");
        //        if (File.Exists(pdbName))
        //        {
        //            byte[] pdbDatas = File.ReadAllBytes(pdbName);
        //            return Assembly.Load(assemblyDatas, pdbDatas);
        //        }
        //        else
        //            return Assembly.Load(assemblyDatas);
        //    }
        //    return null;
        //}
    }
}