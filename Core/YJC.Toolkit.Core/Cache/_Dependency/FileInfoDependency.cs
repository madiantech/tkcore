using System;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    public sealed class FileInfoDependency : ICacheDependency, IDistributeCacheDependency
    {
        private readonly FileInfo fFileInfo;
        private readonly string fFileName;
        private readonly long fLastWriteTime;

        public FileInfoDependency(string fileName)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);

            fFileName = fileName;
            fFileInfo = new FileInfo(fFileName);
            fLastWriteTime = fFileInfo.LastWriteTime.Ticks;
        }

        internal FileInfoDependency(FileInfoStoreConfig config)
        {
            fFileName = config.FileName;
            fFileInfo = new FileInfo(fFileName);
            fLastWriteTime = config.LastWriteTime;
        }

        #region ICacheDependency 成员

        public bool HasChanged
        {
            get
            {
                fFileInfo.Refresh();
                DateTime time = fFileInfo.LastWriteTime;
                return time.Ticks != fLastWriteTime;
            }
        }

        #endregion ICacheDependency 成员

        public object CreateStoredObject()
        {
            return new FileInfoStoreConfig
            {
                FileName = fFileName,
                LastWriteTime = fLastWriteTime
            };
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture,
                "根据文件{0}写入时间判断的缓存依赖", fFileName);
        }
    }
}