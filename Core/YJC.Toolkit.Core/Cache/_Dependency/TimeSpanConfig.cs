using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheDependencyConfig(NamespaceType = NamespaceType.Toolkit, Description = "一段时间内有效的缓存依赖",
        Author = "YJC", CreateDate = "2013-09-28")]
    internal sealed class TimeSpanConfig : IConfigCreator<ICacheDependency>
    {
        [SimpleAttribute]
        public TimeSpan Span { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool SystemConfiged { get; private set; }

        #region IConfigCreator<ICacheDependency> 成员

        public ICacheDependency CreateObject(params object[] args)
        {
            if (SystemConfiged)
            {
                TkDebug.ThrowIfNoAppSetting();
                return new TimeSpanDependency(BaseAppSetting.Current.CacheTime);
            }
            else
                return new TimeSpanDependency(Span);
        }

        #endregion
    }
}
