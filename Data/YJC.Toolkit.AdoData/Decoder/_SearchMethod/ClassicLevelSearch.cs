using System;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class ClassicLevelSearch : ISearch
    {
        private readonly LevelTreeDefinition fTreeDef;
        private readonly ILevelProvider fLevelProvider;

        public ClassicLevelSearch(LevelTreeDefinition treeDef, ILevelProvider levelProvider)
        {
            fLevelProvider = levelProvider;
            fTreeDef = treeDef;
        }

        #region ISearch 成员

        public IParamBuilder Search(EasySearch easySearch, SearchField searchType,
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
                    return GetQueryParamBuilder(context, easySearch.ValueField, fieldValue);
                case SearchField.Name:
                    return SimpleSearch.Instance.Search(easySearch, searchType,
                        context, easySearch.NameField, fieldValue);
                case SearchField.Pinyin:
                    if (easySearch.PinyinField == null)
                        return ClassicPYSearch.Instance.Search(easySearch, searchType,
                            context, easySearch.NameField, fieldValue);
                    else
                        return LikeSearch.Instance.Search(easySearch, searchType,
                            context, easySearch.PinyinField, fieldValue);
            }
            TkDebug.ThrowImpossibleCode(this);
            return null;
        }

        #endregion

        private IParamBuilder GetQueryParamBuilder(TkDbContext context, IFieldInfo fieldName,
            string fieldValue)
        {
            int len = Math.Min(fieldValue.Length, fTreeDef.TotalCount);
            if (len == fTreeDef.TotalCount)
                return SqlParamBuilder.CreateEqualSql(context, fieldName, fieldValue);
            if (len == 0)
                return SqlParamBuilder.CreateSingleSql(context, fieldName,
                    "LIKE", fLevelProvider.GetSqlLikeValue(fTreeDef, 0, fieldValue));
            else
            {
                int level = fTreeDef.GetLevel(fieldValue);
                string likeValue = fLevelProvider.GetSqlLikeValue(fTreeDef, level + 1, fieldValue);
                string exceptValue = fLevelProvider.GetSqlExceptValue(fTreeDef, level + 1, fieldValue);
                return CodeLikeParamBuilder.CreateLikeSql(context, fieldName, likeValue, exceptValue);
            }
        }
    }
}
