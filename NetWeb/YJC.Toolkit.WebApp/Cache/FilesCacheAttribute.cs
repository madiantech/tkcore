using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class FilesCacheAttribute : CacheDependencyAttribute
    {
        private string[] fFileNames;
        private string fFileList;

        public FilesCacheAttribute(string fileNames, FilePathPosition position)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileNames, "fileNames", null);

            FileNames = fileNames;
            Position = position;

            fFileNames = fileNames.Split(',');
            GetRealName(position);
        }

        /// <summary>
        /// Initializes a new instance of the FilesCacheAttribute class.
        /// </summary>
        /// <param name="fileNames"></param>
        internal FilesCacheAttribute(string[] fileNames, FilePathPosition position)
        {
            fFileNames = fileNames;
            Position = position;
            GetRealName(position);
        }

        private void GetRealName(FilePathPosition position)
        {
            for (int i = 0; i < fFileNames.Length; i++)
            {
                fFileNames[i] = FileUtil.GetRealFileName(fFileNames[i].Trim(), position);
            }
            fFileList = string.Join(", ", fFileNames);
        }

        public string FileNames { get; private set; }

        public FilePathPosition Position { get; private set; }

        protected override ICacheDependency CreateCacheDependency()
        {
            return new FileDependency(fFileNames);
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture,
                "监视以下文件{0}内容是否改变的缓存依赖", fFileList);
        }
    }
}
