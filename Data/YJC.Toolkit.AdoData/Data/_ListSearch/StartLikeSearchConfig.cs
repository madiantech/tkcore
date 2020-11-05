using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    [ListSearchConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-05-10",
        Description = "在模糊查询时，不是使用%Value%的查询方式，而是使用Value%的查询方式")]
    internal class StartLikeSearchConfig : IConfigCreator<BaseListSearch>
    {
        #region IConfigCreator<BaseListSearch> 成员

        public BaseListSearch CreateObject(params object[] args)
        {
            return new StartLikeListSearch();
        }

        #endregion IConfigCreator<BaseListSearch> 成员
    }
}