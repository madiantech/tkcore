using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    [ListSearchConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2019-09-27",
        Description = "MySql下进行正则表达式查询")]
    internal class MySqlRegexSearchConfig : IConfigCreator<BaseListSearch>
    {
        public BaseListSearch CreateObject(params object[] args)
        {
            return new MySqlRegexListSearch();
        }
    }
}