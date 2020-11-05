using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-10-10", Description = "空数据源")]
    internal class EmptySourceConfig : IConfigCreator<ISource>
    {
        [SimpleAttribute]
        public bool UseCallerInfo { get; private set; }

        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            return new EmptySource(UseCallerInfo);
        }

        #endregion
    }
}
