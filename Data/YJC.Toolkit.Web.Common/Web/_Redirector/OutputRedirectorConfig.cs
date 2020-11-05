using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [RedirectorConfig(Description = "将Source返回值直接作为Url重定向",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-10-11")]
    [CacheInstance, AlwaysCache]
    internal sealed class OutputRedirectorConfig : IConfigCreator<IRedirector>
    {
        #region IConfigCreator<IRedirector> 成员

        public IRedirector CreateObject(params object[] args)
        {
            return OutputRedirector.Redirector;
        }

        #endregion
    }
}
