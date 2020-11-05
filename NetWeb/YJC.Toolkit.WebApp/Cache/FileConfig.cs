using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheDependencyConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        Description = "跟踪单个文件变化的缓存依赖", CreateDate = "2013-10-11")]
    internal sealed class FileConfig : IConfigCreator<ICacheDependency>
    {
        [SimpleAttribute]
        public string FileName { get; private set; }

        [SimpleAttribute(DefaultValue = FilePathPosition.Application)]
        public FilePathPosition Position { get; private set; }

        #region IConfigCreator<CacheDependencyAttribute> 成员

        ICacheDependency IConfigCreator<ICacheDependency>.CreateObject(params object[] args)
        {
            return new FileDependency(FileUtil.GetRealFileName(FileName, Position));
        }

        #endregion
    }
}
