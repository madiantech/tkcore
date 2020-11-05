using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [RedirectorConfig(Description = "根据QueryString的RetUrl进行重定向，如果没有重定向到首页",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-10-14")]
    [CacheInstance, AlwaysCache]
    internal sealed class RetUrlRedirectorConfig : IConfigCreator<IRedirector>
    {
        #region IConfigCreator<IRedirector> 成员

        public IRedirector CreateObject(params object[] args)
        {
            return RetUrlRedirector.Redirector;
        }

        #endregion
    }
}
