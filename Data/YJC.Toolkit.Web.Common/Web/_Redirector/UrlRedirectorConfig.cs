using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [RedirectorConfig(Description = "根据配置的Url进行重定向",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-10-11")]
    internal sealed class UrlRedirectorConfig : IConfigCreator<IRedirector>
    {
        [SimpleAttribute]
        public string Url { get; private set; }

        #region IConfigCreator<IRedirector> 成员

        public IRedirector CreateObject(params object[] args)
        {
            return new UrlRedirector(Url);
        }

        #endregion
    }
}
