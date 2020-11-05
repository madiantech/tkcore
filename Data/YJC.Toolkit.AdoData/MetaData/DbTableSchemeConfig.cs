using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [TableSchemeConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-08-20",
        Author = "YJC", Description = "从数据库提取表信息得到的单表Scheme")]
    class DbTableSchemeConfig : IConfigCreator<ITableScheme>
    {
        #region IConfigCreator<ITableScheme> 成员

        public ITableScheme CreateObject(params object[] args)
        {
            TkDbContext context = string.IsNullOrEmpty(Context) ? DbContextUtil.CreateDefault() :
                DbContextUtil.CreateDbContext(Context);

            using (context)
            {
                if (!string.IsNullOrEmpty(KeyFields) && !string.IsNullOrEmpty(Fields))
                    return DbUtil.CreateTableScheme(TableName, KeyFields, Fields, context);
                else if (!string.IsNullOrEmpty(KeyFields))
                    return DbUtil.CreateTableScheme(TableName, KeyFields, context);
                else
                    return DbUtil.CreateTableScheme(TableName, context);
            }
        }

        #endregion

        [SimpleAttribute]
        public string Context { get; private set; }

        [SimpleAttribute]
        public string TableName { get; private set; }

        [SimpleAttribute]
        public string KeyFields { get; private set; }

        [SimpleAttribute]
        public string Fields { get; private set; }
    }
}
