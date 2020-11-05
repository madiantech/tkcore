using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class SqlParamBuilder : ParamBuilder
    {
        private SqlParamBuilder()
        {
        }

        public static IParamBuilder CreateSingleSql(TkDbContext context, string fieldName,
            TkDataType type, string oper, object fieldValue)
        {
            return CreateSingleSql(context, fieldName, type, oper, fieldName, fieldValue);
        }

        public static IParamBuilder CreateSingleSql(TkDbContext context, IFieldInfo field,
            string oper, object fieldValue)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            return CreateSingleSql(context, field.FieldName, field.DataType, oper, fieldValue);
        }

        public static IParamBuilder CreateSingleSql(TkDbContext context, string fieldName,
            TkDataType type, string oper, string paramName, object fieldValue)
        {
            return InternalCreateSingleSql(context, fieldName, type, oper, paramName, fieldValue);
        }

        public static IParamBuilder CreateSingleSql(TkDbContext context, IFieldInfo field,
            string oper, string paramName, object fieldValue)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            return CreateSingleSql(context, field.FieldName, field.DataType, oper, paramName, fieldValue);
        }


        public static IParamBuilder CreateEqualSql(TkDbContext context, string fieldName,
            TkDataType type, object fieldValue)
        {
            return CreateSingleSql(context, fieldName, type, "=", fieldValue);
        }

        public static IParamBuilder CreateEqualSql(TkDbContext context, IFieldInfo field, object fieldValue)
        {
            return CreateSingleSql(context, field, "=", fieldValue);
        }

        public static IParamBuilder CreateEqualSql(TkDbContext context, string fieldName,
            TkDataType type, string paramName, object fieldValue)
        {
            return CreateSingleSql(context, fieldName, type, "=", paramName, fieldValue);
        }

        public static IParamBuilder CreateEqualSql(TkDbContext context, IFieldInfo field,
            string paramName, object fieldValue)
        {
            return CreateSingleSql(context, field, "=", paramName, fieldValue);
        }

        public static IParamBuilder CreateInSql(TkDbContext context, IFieldInfo fieldName,
            IEnumerable<string> values)
        {
            TkDebug.AssertArgumentNull(fieldName, "fieldName", null);
            TkDebug.AssertArgumentNull(values, "values", null);

            string[] valueArray = values.ToArray();
            if (valueArray.Length == 0)
                return ParamBuilder.NoResult;

            string seperator = string.Empty;
            switch (fieldName.DataType)
            {
                case TkDataType.String:
                    seperator = "\'";
                    break;
                case TkDataType.Int:
                case TkDataType.Byte:
                case TkDataType.Short:
                case TkDataType.Double:
                case TkDataType.Long:
                    break;
                default:
                    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                        "生成IN SQL语句的字段只支持字符串、整形、浮点三种类型，现在字段{0}类型是{1}，不在支持范围",
                        fieldName.FieldName, fieldName.DataType), null);
                    break;
            }
            var items = from value in valueArray
                        select seperator + StringUtil.EscapeAposString(value) + seperator;
            string insql = string.Join(",", items);
            string field;
            if (context != null)
                field = context.EscapeName(fieldName.FieldName);
            else
                field = fieldName.FieldName;
            string sql = string.Format(ObjectUtil.SysCulture, "{0} IN ({1})",
                field, insql);
            InternalParamBuilder builder = new InternalParamBuilder { Sql = sql };
            return builder;
        }
    }
}
