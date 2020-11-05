using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class FileCacheAttribute : CacheDependencyAttribute
    {
        /// <summary>
        /// Initializes a new instance of the FileCacheDependencyAttribute class.
        /// </summary>
        /// <param name="fileName"></param>
        public FileCacheAttribute(string fileName, FilePathPosition position)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);

            FileName = fileName;
            Position = position;
            FileName = FileUtil.GetRealFileName(fileName, position);
        }

        public string FileName { get; private set; }

        public FilePathPosition Position { get; private set; }

        protected override ICacheDependency CreateCacheDependency()
        {
            return new FileDependency(FileName);
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture,
                "监视文件{0}内容是否改变的缓存依赖", FileName);
        }
    }
}
