using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    [ListSearchConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-05-10",
        Description = "默认的查询")]
    internal class DefaultSearchConfig : IConfigCreator<BaseListSearch>
    {
        #region IConfigCreator<BaseListSearch> 成员

        public BaseListSearch CreateObject(params object[] args)
        {
            return new DefaultListSearch
            {
                WordSplit = WordSplit
            };
        }

        #endregion IConfigCreator<BaseListSearch> 成员

        [SimpleAttribute]
        public bool WordSplit { get; private set; }
    }
}