using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-10-17", Description = "执行配置的Sql语句来获取数据源")]
    internal sealed class SqlSourceConfig : IConfigCreator<ISource>
    {
        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            return new SqlSource(this);
        }

        #endregion

        [SimpleAttribute]
        public DbContextConfig Context { get; private set; }

        [SimpleAttribute]
        public bool UseCallerInfo { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Sql")]
        public List<SqlSourceSqlConfig> Sqls { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText SuccessMessage { get; private set; }
    }
}
