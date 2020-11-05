using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class LikeParamBuilder : IParamBuilder
    {
        internal static char[] WIDE_CHARS = { '%', '_' };

        private string fSql;
        private readonly DbParameterList fParams;
        private bool fIsEscape;

        /// <summary>
        /// Initializes a new instance of the LikeParamBuilder class.
        /// </summary>
        private LikeParamBuilder()
        {
            fParams = new DbParameterList();
        }

        #region IParamBuilder 成员

        string IParamBuilder.Sql
        {
            get
            {
                return fSql;
            }
        }

        DbParameterList IParamBuilder.Parameters
        {
            get
            {
                return fParams;
            }
        }

        #endregion

        private IParamBuilder InternalCreateLikeSql(TkDbContext context, IFieldInfo fieldInfo, string paramName, string fieldValue)
        {
            fIsEscape = fieldValue.IndexOfAny(WIDE_CHARS) != -1;
            string fieldName = context.EscapeName(fieldInfo.FieldName);
            string fieldName1 = paramName + "1";
            string fieldName2 = paramName + "2";

            if (fIsEscape)
            {
                fSql = string.Format(ObjectUtil.SysCulture,
                    "(({0} LIKE {1} ESCAPE '\\') OR ({0} LIKE {2} ESCAPE '\\'))", fieldName,
                    context.GetSqlParamName(fieldName1), context.GetSqlParamName(fieldName2));
                fieldValue = StringUtil.EscapeSqlString(fieldValue);
            }
            else
                fSql = string.Format(ObjectUtil.SysCulture,
                    "(({0} LIKE {1}) OR ({0} LIKE {2}))", fieldName,
                    context.GetSqlParamName(fieldName1), context.GetSqlParamName(fieldName2));

            fParams.Add(fieldName1, fieldInfo.DataType, fieldValue + "%");
            fParams.Add(fieldName2, fieldInfo.DataType, "%" + fieldValue + "%");
            return this;
        }

        public static IParamBuilder CreateLikeSql(TkDbContext context,
            IFieldInfo fieldName, string fieldValue)
        {
            LikeParamBuilder builder = new LikeParamBuilder();
            return builder.InternalCreateLikeSql(context, fieldName, fieldName.FieldName, fieldValue);
        }


        public static IParamBuilder CreateLikeSql(TkDbContext context,
            IFieldInfo fieldName, string paramName, string fieldValue)
        {
            LikeParamBuilder builder = new LikeParamBuilder();
            return builder.InternalCreateLikeSql(context, fieldName, paramName, fieldValue);
        }
    }
}
