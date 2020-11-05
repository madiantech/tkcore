using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheDependencyConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        Description = "跟踪多个文件任一文件发生变化的缓存依赖", CreateDate = "2013-10-11")]
    internal sealed class FilesConfig : IConfigCreator<CacheDependencyAttribute>
    {
        [SimpleAttribute]
        public string FileNames { get; private set; }

        [SimpleAttribute(DefaultValue = FilePathPosition.Application)]
        public FilePathPosition Position { get; private set; }

        #region IConfigCreator<CacheDependencyAttribute> 成员

        CacheDependencyAttribute IConfigCreator<CacheDependencyAttribute>.CreateObject(params object[] args)
        {
            return new FilesCacheAttribute(FileNames, Position);
        }

        #endregion
    }
}
