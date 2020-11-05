using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-10-10", Description = "配置一段静态文本作为数据源")]
    internal class StaticSourceConfig : IConfigCreator<ISource>
    {
        [TextContent]
        public string Content { get; private set; }

        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            return new StaticSource(this);
        }

        #endregion
    }
}
