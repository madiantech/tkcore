using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal sealed class CodeLikeParamBuilder : IParamBuilder
    {
        private string fSql;
        private readonly DbParameterList fParams;

        private CodeLikeParamBuilder()
        {
            fParams = new DbParameterList();
        }

        #region IParamBuilder 成员

        public string Sql
        {
            get
            {
                return fSql;
            }
        }

        public DbParameterList Parameters
        {
            get
            {
                return fParams;
            }
        }

        #endregion

        private IParamBuilder InternalCreateLikeSql(TkDbContext context, IFieldInfo fieldName,
           string likeValue, string exceptValue)
        {
            string fieldName1 = fieldName.FieldName + "1";
            string fieldName2 = fieldName.FieldName + "2";
            fSql = string.Format(ObjectUtil.SysCulture, "{0} LIKE {1} AND {0} <> {2}",
                context.EscapeName(fieldName.FieldName), context.GetSqlParamName(fieldName1),
                context.GetSqlParamName(fieldName2));

            fParams.Add(fieldName1, fieldName.DataType, likeValue);
            fParams.Add(fieldName2, fieldName.DataType, exceptValue);

            return this;
        }

        public static IParamBuilder CreateLikeSql(TkDbContext context, IFieldInfo fieldName,
            string likeValue, string exceptValue)
        {
            CodeLikeParamBuilder builder = new CodeLikeParamBuilder();
            return builder.InternalCreateLikeSql(context, fieldName, likeValue, exceptValue);
        }
    }
}
