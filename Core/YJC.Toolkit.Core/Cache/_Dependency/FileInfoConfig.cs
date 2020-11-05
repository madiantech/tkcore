using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheDependencyConfig(Author = "YJC", NamespaceType = NamespaceType.Toolkit,
        Description = "跟踪单个文件变化(监控文件的修改时间)的缓存依赖", CreateDate = "2013-10-11")]
    internal sealed class FileInfoConfig : IConfigCreator<ICacheDependency>
    {
        [SimpleAttribute]
        public string FileName { get; private set; }

        [SimpleAttribute(DefaultValue = FilePathPosition.Application)]
        public FilePathPosition Position { get; private set; }

        #region IConfigCreator<CacheDependencyAttribute> 成员

        ICacheDependency IConfigCreator<ICacheDependency>.CreateObject(params object[] args)
        {
            return new FileInfoDependency(FileUtil.GetRealFileName(FileName, Position));
        }

        #endregion IConfigCreator<CacheDependencyAttribute> 成员
    }
}