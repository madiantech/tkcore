using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheDependencyConfig(NamespaceType = NamespaceType.Toolkit, Description = "在自然日切换时无效的缓存依赖",
        Author = "YJC", CreateDate = "2013-09-28")]
    internal sealed class DayChangeConfig : IConfigCreator<ICacheDependency>
    {
        [SimpleAttribute(DefaultValue = 1)]
        public int Days { get; private set; }

        #region IConfigCreator<ICacheDependency> 成员

        ICacheDependency IConfigCreator<ICacheDependency>.CreateObject(params object[] args)
        {
            return new DayChangeDependency(Days);
        }

        #endregion
    }
}
