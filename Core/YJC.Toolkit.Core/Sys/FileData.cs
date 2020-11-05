using System.IO;
using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Sys
{
    public sealed class FileData : IRegName, ICacheDependencyCreator
    {
        private readonly FileInfoDependency fDependency;

        internal FileData()
        {
        }

        internal FileData(string fileName)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);
            TkDebug.AssertArgument(File.Exists(fileName), "fileName",
                string.Format(ObjectUtil.SysCulture, "{0}不存在", fileName), null);

            FileName = fileName;
            fDependency = new FileInfoDependency(fileName);
            Data = File.ReadAllBytes(fileName);
        }

        #region ICacheDependencyCreator 成员

        public ICacheDependency CreateCacheDependency()
        {
            return fDependency;
        }

        #endregion ICacheDependencyCreator 成员

        #region IRegName 成员

        string IRegName.RegName
        {
            get
            {
                return FileName;
            }
        }

        #endregion IRegName 成员

        [SimpleElement]
        public string FileName { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        [SimpleElement]
        public byte[] Data { get; private set; }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "{0}的文件内容", FileName);
        }

        public static FileData Create(string fileName)
        {
            FileData content = CacheManager.GetItem("FileContent",
                fileName).Convert<FileData>();
            return content;
        }
    }
}