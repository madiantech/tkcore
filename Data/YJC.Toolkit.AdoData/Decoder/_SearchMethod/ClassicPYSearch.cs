using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [InstancePlugIn, AlwaysCache]
    [Search(Description = "拼音查询", Author = "YJC", CreateDate = "2009-04-26")]
    internal sealed class ClassicPYSearch : ISearch
    {
        internal static readonly ISearch Instance = new ClassicPYSearch();

        internal static readonly char[] WideChars = { '%', '_' };

        private ClassicPYSearch()
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

            fieldValue = StringUtil.EscapeAposString(fieldValue);
            string sql;
            if (fieldValue.IndexOfAny(WideChars) != -1)
            {
                fieldValue = StringUtil.EscapeSqlString(fieldValue);
                sql = string.Format(ObjectUtil.SysCulture,
                    "(({0} LIKE '{1}%' ESCAPE '\\') OR ({0} LIKE '{2}%' ESCAPE '\\') OR ({3}))",
                    context.EscapeName(fieldName.FieldName), fieldValue.ToUpper(ObjectUtil.SysCulture),
                    fieldValue.ToLower(ObjectUtil.SysCulture),
                    PinYinUtil.GetCharFullCondition(context, fieldName.FieldName, fieldValue));
            }
            else
                sql = string.Format(ObjectUtil.SysCulture,
                    "(({0} LIKE '{1}%') OR ({0} LIKE '{2}%') OR ({3}))",
                    context.EscapeName(fieldName.FieldName), fieldValue.ToUpper(ObjectUtil.SysCulture),
                    fieldValue.ToLower(ObjectUtil.SysCulture),
                    PinYinUtil.GetCharFullCondition(context, fieldName.FieldName, fieldValue));
            return SqlParamBuilder.CreateSql(sql);
        }

        #endregion
    }
}
