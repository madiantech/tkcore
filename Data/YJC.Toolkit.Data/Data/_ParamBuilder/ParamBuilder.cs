using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ParamBuilder
    {
        public static readonly IParamBuilder NoResult = CreateSql("1 = 0");

        protected internal ParamBuilder()
        {
        }

        protected internal static string CreateSingleSql(TkDbContext context, string fieldName,
            string oper, string paramName)
        {
            string sql = string.Format(ObjectUtil.SysCulture, "{0} {1} {2}",
                context.EscapeName(fieldName), oper, context.GetSqlParamName(paramName));
            return sql;
        }

        internal static IParamBuilder InternalCreateSingleSql(TkDbContext context, string fieldName,
            TkDataType type, string oper, string paramName, object fieldValue)
        {
            TkDebug.AssertArgumentNull(context, "context", null);
            TkDebug.AssertArgumentNullOrEmpty(fieldName, "fieldName", null);
            TkDebug.AssertArgumentNullOrEmpty(oper, "oper", null);
            TkDebug.AssertArgumentNullOrEmpty(paramName, "paramName", null);
            TkDebug.AssertArgumentNull(fieldValue, "fieldValue", null);

            InternalParamBuilder builder = new InternalParamBuilder
            {
                Sql = CreateSingleSql(context, fieldName, oper, paramName)
            };
            builder.Parameters.Add(paramName, type, fieldValue);
            return builder;
        }

        public static IParamBuilder CreateSql(string sql)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", null);

            return new InternalParamBuilder { Sql = sql };
        }

        private static IParamBuilder LinkParamBuilder(IEnumerable<IParamBuilder> builders,
            string linkOperator)
        {
            int j = 0;
            InternalParamBuilder result = new InternalParamBuilder();
            StringBuilder builder = new StringBuilder();
            foreach (IParamBuilder item in builders)
            {
                string sql = item.Sql;
                TkDebug.AssertNotNullOrEmpty(sql, string.Format(ObjectUtil.SysCulture,
                    "builders参数第{0}个IParamBuilder生成的Sql是空，这是不允许的", j), item);
                if (j > 0)
                    builder.Append(linkOperator);
                builder.Append("(").Append(sql).Append(")");
                //StringUtil.JoinStringItem(builder, i++, "(" + sql + ")", linkOperator);
                DbParameterList itemParams = item.Parameters;
                TkDebug.AssertNotNull(itemParams, string.Format(ObjectUtil.SysCulture,
                    "builders参数第{0}个IParamBuilder调用的Parameters返回是空，这是不允许的",
                    j), item);
                result.Parameters.Add(itemParams);
                ++j;
            }
            //result.Sql = builder.ToString();
            if (j == 0)
                return null;
            else if (j == 1)
                return builders.First();
            else
            {
                result.Sql = builder.ToString();
                return result;
            }
        }

        public static IParamBuilder CreateParamBuilder(string filterSql, params IParamBuilder[] builders)
        {
            TkDebug.AssertArgumentNullOrEmpty(filterSql, "filterSql", null);

            IParamBuilder builder = CreateSql(filterSql);
            return CreateParamBuilder(EnumUtil.Convert(builder, builders));
        }

        public static IParamBuilder CreateParamBuilder(params IParamBuilder[] builders)
        {
            TkDebug.AssertArgumentNull(builders, "builders", null);

            var itemBuilders = from builder in builders
                               where builder != null
                               select builder;
            return LinkParamBuilder(itemBuilders, " AND ");
        }

        public static IParamBuilder CreateParamBuilder(IEnumerable<IParamBuilder> builders)
        {
            TkDebug.AssertArgumentNull(builders, "builders", null);

            var itemBuilders = from builder in builders
                               where builder != null
                               select builder;
            return LinkParamBuilder(itemBuilders, " AND ");
        }

        public static IParamBuilder CreateParamBuilderWithOr(string filterSql, params IParamBuilder[] builders)
        {
            TkDebug.AssertArgumentNullOrEmpty(filterSql, "filterSql", null);

            IParamBuilder builder = CreateSql(filterSql);
            return CreateParamBuilderWithOr(EnumUtil.Convert(builder, builders));
        }

        public static IParamBuilder CreateParamBuilderWithOr(params IParamBuilder[] builders)
        {
            TkDebug.AssertArgumentNull(builders, "builders", null);

            var itemBuilders = from builder in builders
                               where builder != null
                               select builder;
            return LinkParamBuilder(itemBuilders, " OR ");
        }

        public static IParamBuilder CreateParamBuilderWithOr(IEnumerable<IParamBuilder> builders)
        {
            TkDebug.AssertArgumentNull(builders, "builders", null);

            var itemBuilders = from builder in builders
                               where builder != null
                               select builder;
            return LinkParamBuilder(itemBuilders, " OR ");
        }

        public static IParamBuilder CreateParamBuilder(string sql, DbParameterList paramList)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", null);
            TkDebug.AssertArgumentNull(paramList, "paramList", null);

            return new InternalParamBuilder(sql, paramList);
        }

        public static IParamBuilder CreateParamBuilder(string sql, string fieldName,
            TkDataType dataType, object fieldValue)
        {
            TkDebug.AssertArgumentNullOrEmpty(fieldName, "fieldName", null);

            DbParameterList paramList = new DbParameterList();
            paramList.Add(fieldName, dataType, fieldValue);
            return CreateParamBuilder(sql, paramList);
        }

        public static IParamBuilder CreateParamBuilder(string sql, IFieldInfo fieldInfo, object fieldValue)
        {
            DbParameterList paramList = new DbParameterList();
            paramList.Add(fieldInfo, fieldValue);
            return CreateParamBuilder(sql, paramList);
        }
    }
}