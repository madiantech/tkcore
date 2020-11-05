using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheDependencyConfig(NamespaceType = NamespaceType.Toolkit, Description = "活跃时间的缓存依赖",
        Author = "YJC", CreateDate = "2013-09-28")]
    internal sealed class ActiveTimeConfig : IConfigCreator<ICacheDependency>
    {
        [SimpleAttribute]
        public TimeSpan Span { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool SystemConfiged { get; private set; }

        #region IConfigCreator<ICacheDependency> 成员

        ICacheDependency IConfigCreator<ICacheDependency>.CreateObject(params object[] args)
        {
            if (SystemConfiged)
            {
                TkDebug.ThrowIfNoAppSetting();
                return new ActiveTimeDependency(BaseAppSetting.Current.CacheTime);
            }
            else
                return new ActiveTimeDependency(Span);
        }

        #endregion
    }
}
