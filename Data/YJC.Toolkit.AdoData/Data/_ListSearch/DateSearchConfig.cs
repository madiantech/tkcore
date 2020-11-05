using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    [ListSearchConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-05-10",
        Description = "默认的日期查询")]
    internal class DateSearchConfig : IConfigCreator<BaseListSearch>
    {
        #region IConfigCreator<BaseListSearch> 成员

        public BaseListSearch CreateObject(params object[] args)
        {
            return new DefaultDateSearch();
        }

        #endregion IConfigCreator<BaseListSearch> 成员
    }
}