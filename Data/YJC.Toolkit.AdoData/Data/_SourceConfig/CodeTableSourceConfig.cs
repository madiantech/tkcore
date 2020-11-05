using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2018-12-14", Description = "提供CodeTable的数据源")]
    internal class CodeTableSourceConfig : IConfigCreator<ISource>
    {
        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            CodeTableSource source = new CodeTableSource(RegName);
            if (!string.IsNullOrEmpty(Context))
                source.Context = DbContextUtil.CreateDbContext(Context);

            return source;
        }

        #endregion IConfigCreator<ISource> 成员

        [SimpleAttribute(Required = true)]
        public string RegName { get; private set; }

        [SimpleAttribute]
        public string Context { get; private set; }
    }
}