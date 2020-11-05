using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [InstancePlugIn, AlwaysCache]
    [Search(Description = "对查询的值做LIKE，两边加%",
        Author = "YJC", CreateDate = "2009-04-25")]
    internal sealed class SimpleSearch : ISearch
    {
        internal static readonly ISearch Instance = new SimpleSearch();

        private SimpleSearch()
        {
        }

        #region ISearch 成员

        IParamBuilder ISearch.Search(EasySearch easySearch, SearchField searchType,
            TkDbContext context, IFieldInfo fieldName, string fieldValue)
        {
            TkDebug.AssertArgumentNull(context, "context", this);
            TkDebug.AssertArgumentNull(fieldName, "fieldName", this);

            if (string.IsNullOrEmpty(fieldValue))
                return null;
            else
                return SqlParamBuilder.CreateSingleSql(context, fieldName, "LIKE",
                    string.Format(ObjectUtil.SysCulture, "%{0}%", fieldValue));
        }

        #endregion
    }
}
