using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [InstancePlugIn, AlwaysCache]
    [Search(Author = "YJC", CreateDate = "2009-04-26",
        Description = "对代码表进行查询，要求代码表必须有CODE_VALUE,CODE_NAME,CODE_PY三个字段")]
    internal sealed class CodeTableSearch : ISearch
    {
        internal static readonly ISearch Instance = new CodeTableSearch();

        private CodeTableSearch()
        {
        }

        #region ISearch 成员

        IParamBuilder ISearch.Search(EasySearch easySearch, SearchField searchType,
            TkDbContext context, IFieldInfo fieldName, string fieldValue)
        {
            TkDebug.AssertArgumentNull(easySearch, "easySearch", this);
            TkDebug.AssertArgumentNull(context, "context", this);
            TkDebug.AssertArgumentNull(fieldName, "fieldName", this);

            switch (searchType)
            {
                case SearchField.Value:
                case SearchField.DefaultValue:
                    return LikeSearch.Instance.Search(easySearch, searchType, context,
                        easySearch.ValueField, fieldValue);
                case SearchField.Name:
                    return SimpleSearch.Instance.Search(easySearch, searchType, context,
                        easySearch.NameField, fieldValue);
                case SearchField.Pinyin:
                    return LikeSearch.Instance.Search(easySearch, searchType, context,
                        easySearch.PinyinField, fieldValue.ToUpper(ObjectUtil.SysCulture));
            }
            TkDebug.ThrowImpossibleCode(this);
            return null;
        }

        #endregion
    }
}
