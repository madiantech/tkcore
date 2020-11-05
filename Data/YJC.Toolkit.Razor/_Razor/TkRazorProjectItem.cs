using Microsoft.Extensions.Primitives;
using System.IO;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public abstract class TkRazorProjectItem
    {
        protected TkRazorProjectItem()
        {
        }

        public IChangeToken ExpirationToken { get; set; }

        public abstract string Key { get; }

        public abstract bool Exists { get; }

        public abstract string BaseAssemblyPath { get; }

        public abstract Stream Read();

        public abstract string CreateAssemblyName();

        public Assembly TryLoadAssembly()
        {
            string path = GetPath();
            string assemblyName = Path.Combine(path, CreateAssemblyName() + ".dll");
            if (File.Exists(assemblyName))
            {
                byte[] assemblyDatas = File.ReadAllBytes(assemblyName);
                string pdbName = Path.Combine(path, CreateAssemblyName() + ".pdb");
                if (File.Exists(pdbName))
                {
                    byte[] pdbDatas = File.ReadAllBytes(pdbName);
                    return Assembly.Load(assemblyDatas, pdbDatas);
                }
                else
                    return Assembly.Load(assemblyDatas);
            }
            return null;
        }

        public void SaveAssembly(Stream assembly, Stream pdb)
        {
            string path = GetPath();
            string assemblyName = CreateAssemblyName();
            SaveStream(Path.Combine(path, assemblyName + ".dll"), assembly);
            SaveStream(Path.Combine(path, assemblyName + ".pdb"), pdb);
        }

        private string GetPath()
        {
            string result = Path.Combine(BaseAppSetting.Current.XmlPath, @"razor/temp", BaseAssemblyPath);
            if (!Directory.Exists(result))
                Directory.CreateDirectory(result);
            return result;
        }

        private void SaveStream(string fileName, Stream stream)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            stream.Seek(0, SeekOrigin.Begin);
            using (fileStream)
            {
                FileUtil.CopyStream(stream, fileStream);
            }
        }
    }
}