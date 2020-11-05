using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    [ListSearchConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-05-10",
        Description = "精确查询")]
    internal class EqualSearchConfig : IConfigCreator<BaseListSearch>
    {
        #region IConfigCreator<BaseListSearch> 成员

        public BaseListSearch CreateObject(params object[] args)
        {
            return new DefaultEqualSearch();
        }

        #endregion IConfigCreator<BaseListSearch> 成员
    }
}