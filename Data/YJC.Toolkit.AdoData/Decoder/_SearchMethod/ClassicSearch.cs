using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [InstancePlugIn, AlwaysCache]
    [Search(Description = "默认EasySearch的搜索方式",
        Author = "YJC", CreateDate = "2009-04-26")]
    internal sealed class ClassicSearch : ISearch
    {
        internal static readonly ISearch Instance = new ClassicSearch();

        private ClassicSearch()
        {
        }

        #region ISearch 成员

        IParamBuilder ISearch.Search(EasySearch easySearch, SearchField searchType,
            TkDbContext context, IFieldInfo fieldName, string fieldValue)
        {
            TkDebug.AssertArgumentNull(easySearch, "easySearch", this);
            TkDebug.AssertArgumentNull(context, "context", this);
            TkDebug.AssertArgumentNull(fieldName, "fieldName", this);
            TkDebug.AssertArgumentNull(fieldValue, "fieldValue", this);

            switch (searchType)
            {
                case SearchField.Value:
                case SearchField.DefaultValue:
                    return LikeSearch.Instance.Search(easySearch, searchType,
                        context, easySearch.NameField, fieldValue);
                case SearchField.Name:
                    return SimpleSearch.Instance.Search(easySearch, searchType,
                        context, easySearch.NameField, fieldValue);
                case SearchField.Pinyin:
                    if (easySearch.PinyinField == null)
                        return ClassicPYSearch.Instance.Search(easySearch, searchType,
                            context, easySearch.NameField, fieldValue);
                    else
                        return LikeSearch.Instance.Search(easySearch, searchType,
                            context, easySearch.PinyinField, fieldValue.ToUpper(ObjectUtil.SysCulture));
            }
            TkDebug.ThrowImpossibleCode(this);
            return null;
        }

        #endregion
    }
}
