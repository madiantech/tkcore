using YJC.Toolkit.Decoder;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [TableSchemeConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-08-20",
        Author = "YJC", Description = "获取标准代码表的单表Scheme")]
    class StandardCodeTableSchemeConfig : IConfigCreator<ITableScheme>
    {
        #region IConfigCreator<ITableScheme> 成员

        public ITableScheme CreateObject(params object[] args)
        {
            return new StandardCodeTableScheme(TableName);
        }

        #endregion

        [SimpleAttribute]
        public string TableName { get; private set; }
    }
}
