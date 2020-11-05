using System.Collections.Generic;
using System.Data;
using System.Text;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal static class SqlBuilder
    {
        private const int BUFFER_SIZE = 2048;

        public static void GetSelectCommand(TableSelector selector)
        {
            SetSelectCommand(selector);
            selector.DataAdapter.SelectCommand.CommandText = selector.SkeletonSelectSql;
        }

        //private static string CreateSelectSql(TableSelector selector)
        //{
        //    return string.Format(ObjectUtil.SysCulture,
        //        "SELECT {0} FROM {1}", selector.Fields, selector.Context.EscapeName(selector.TableName));
        //}

        private static void SetSelectCommand(ISqlDataAdapter selector)
        {
            if (selector.DataAdapter.SelectCommand == null)
                selector.DataAdapter.SelectCommand = selector.Context.CreateCommand();
            else
                selector.DataAdapter.SelectCommand.Parameters.Clear();
        }

        //public static void GetSelectCommandWithParams(TableSelector selector, IFieldInfo[] fields, object[] values)
        //{
        //    SetSelectCommand(selector);
        //    selector.DataAdapter.SelectCommand.CommandText = CreateSelectParamSql(selector, fields, values);
        //}

        //public static void GetSelectCommandWithParams(TableSelector selector, string filterSql, string orderBy, IFieldInfo[] fields, object[] values)
        //{
        //    SetSelectCommand(selector);
        //    selector.DataAdapter.SelectCommand.CommandText = CreateSelectParamSql(selector, filterSql, orderBy, fields, values);
        //}

        //private static string CreateSelectParamSql(TableSelector selector, IFieldInfo[] fields, object[] values)
        //{
        //    return string.Format(ObjectUtil.SysCulture, "SELECT {0} FROM {1} {2}", selector.Fields, selector.TableName,
        //        CreateWhereSql(selector, selector.DataAdapter.SelectCommand, false, string.Empty, fields, values));
        //}

        //private static string CreateSelectParamSql(TableSelector selector, string filterSql, string orderBy, IFieldInfo[] fields, object[] values)
        //{
        //    if (!string.IsNullOrEmpty(orderBy))
        //        orderBy = " ORDER BY " + orderBy;
        //    return string.Format(ObjectUtil.SysCulture, "SELECT {0} FROM {1} {2}{3}", selector.Fields, selector.TableName,
        //        CreateWhereSql(selector, selector.DataAdapter.SelectCommand, false, filterSql, fields, values), orderBy);
        //}

        //private static string CreateWhereSql(TableSelector selector, IDbCommand command,
        //    bool isOrigin, string filterSql, IFieldInfo[] fields, object[] values)
        //{
        //    TkDebug.AssertArgument(values == null || (values != null & fields.Length == values.Length), "values",
        //        string.Format(ObjectUtil.SysCulture, "参数fields和values的长度个数不相等，fields的个数为{0}，values的个数为{1}",
        //        fields.Length, values.Length), selector);
        //    StringBuilder whereSQL = new StringBuilder(BUFFER_SIZE);
        //    int i = 0;
        //    if (!string.IsNullOrEmpty(filterSql))
        //        JoinStringItem(whereSQL, i++, "(" + filterSql + ")", " AND ");
        //    int j = 0;
        //    Array.ForEach(fields, field =>
        //    {
        //        JoinStringItem(whereSQL, i, string.Format(ObjectUtil.SysCulture,
        //            "({0} = {1})", field.FieldName,
        //            selector.Context.GetSqlParamName(field.FieldName, isOrigin)), " AND ");
        //        IDbDataParameter parameter = selector.CreateDbDataParameter(field, isOrigin);
        //        if (values != null)
        //            parameter.Value = values[j];
        //        command.Parameters.Add(parameter);
        //        i++;
        //        j++;
        //    });
        //    return "WHERE " + whereSQL;
        //}

        public static void GetSelectCommandSql(ISqlDataAdapter selector, string sql)
        {
            SetSelectCommand(selector);
            selector.DataAdapter.SelectCommand.CommandText = sql;
        }

        public static void GetInsertCommand(TableResolver resolver)
        {
            List<FieldInfoEventArgs> list = resolver.GetFieldInfo(UpdateKind.Insert);
            if (resolver.DataAdapter.InsertCommand == null)
                resolver.DataAdapter.InsertCommand = resolver.Context.CreateCommand();
            else
                resolver.DataAdapter.InsertCommand.Parameters.Clear();
            resolver.DataAdapter.InsertCommand.CommandText = CreateInsertSql(resolver, list);
            list = null;
        }

        private static IDbDataParameter CreateDataParameter(TkDbContext context, IFieldInfo fieldInfo, bool isOrigin)
        {
            IDbDataParameter result = context.CreateParameter(fieldInfo, isOrigin);
            result.SourceColumn = fieldInfo.NickName;
            if (isOrigin)
                result.SourceVersion = DataRowVersion.Original;
            return result;
        }

        private static IDbDataParameter CreateDataParameter(TkDbContext context, IFieldInfo fieldInfo)
        {
            return CreateDataParameter(context, fieldInfo, false);
        }

        internal static void JoinStringItem(StringBuilder builder, int index, string value, string joinStr = ", ")
        {
            if (index > 0)
                builder.Append(joinStr);
            builder.Append(value);
        }

        private static string CreateInsertSql(TableResolver resolver, List<FieldInfoEventArgs> list)
        {
            StringBuilder insertSQL = new StringBuilder(BUFFER_SIZE);
            StringBuilder valuesSQL = new StringBuilder(BUFFER_SIZE);
            int i = 0;
            TkDbContext context = resolver.Context;
            IDataParameterCollection parameters = resolver.DataAdapter.InsertCommand.Parameters;
            foreach (FieldInfoEventArgs item in list)
            {
                if ((item.Position & SqlPosition.Update) == SqlPosition.Update)
                {
                    string fieldName = item.FieldInfo.FieldName;
                    JoinStringItem(insertSQL, i, context.EscapeName(fieldName));
                    JoinStringItem(valuesSQL, i, context.GetSqlParamName(fieldName));
                    parameters.Add(CreateDataParameter(context, item.FieldInfo));
                    ++i;
                }
            }
            return string.Format(ObjectUtil.SysCulture, "INSERT INTO {0} ({1}) VALUES ({2})",
                context.EscapeName(resolver.TableName), insertSQL, valuesSQL);
        }

        public static void GetUpdateCommand(TableResolver resolver)
        {
            List<FieldInfoEventArgs> list = resolver.GetFieldInfo(UpdateKind.Update);
            if (resolver.DataAdapter.UpdateCommand == null)
                resolver.DataAdapter.UpdateCommand = resolver.Context.CreateCommand();
            else
                resolver.DataAdapter.UpdateCommand.Parameters.Clear();
            resolver.DataAdapter.UpdateCommand.CommandText = CreateUpdateSql(resolver, list);
            list = null;
        }

        private static string CreateUpdateSql(TableResolver resolver, List<FieldInfoEventArgs> list)
        {
            StringBuilder setSQL = new StringBuilder(BUFFER_SIZE);
            int i = 0;
            TkDbContext context = resolver.Context;
            IDataParameterCollection parameters = resolver.DataAdapter.UpdateCommand.Parameters;
            foreach (FieldInfoEventArgs item in list)
            {
                if ((item.Position & SqlPosition.Update) == SqlPosition.Update)
                {
                    string fieldName = item.FieldInfo.FieldName;
                    JoinStringItem(setSQL, i++, string.Format(ObjectUtil.SysCulture,
                        "{0} = {1}", context.EscapeName(fieldName), context.GetSqlParamName(fieldName)));
                    parameters.Add(CreateDataParameter(context, item.FieldInfo));
                }
            }

            return string.Format(ObjectUtil.SysCulture, "UPDATE {0} SET {1} {2}",
                context.EscapeName(resolver.TableName), setSQL,
                CreateWhereSql(resolver, resolver.DataAdapter.UpdateCommand, list, true));
        }

        private static string CreateWhereSql(TableResolver resolver, IDbCommand command,
            List<FieldInfoEventArgs> list, bool isOrigin)
        {
            StringBuilder whereSql = new StringBuilder(BUFFER_SIZE);
            int i = 0;
            TkDbContext context = resolver.Context;
            foreach (FieldInfoEventArgs item in list)
            {
                if ((item.Position & SqlPosition.Where) == SqlPosition.Where)
                {
                    string fieldName = item.FieldInfo.FieldName;
                    JoinStringItem(whereSql, i++, string.Format(ObjectUtil.SysCulture, "{0} = {1}",
                        context.EscapeName(fieldName), context.GetSqlParamName(fieldName, isOrigin)),
                        " AND ");
                    command.Parameters.Add(CreateDataParameter(context, item.FieldInfo, isOrigin));
                }
            }
            string sql = whereSql.ToString();
            TkDebug.Assert(!string.IsNullOrEmpty(sql), string.Format(ObjectUtil.SysCulture,
                "表{0}在设置提交SQL的Where部分时，没有找到对应的字段，请确认主键是否设置，或者SetFieldInfo事件是否正确",
                resolver.TableName), resolver);
            return "WHERE " + sql;
        }

        public static void GetDeleteCommand(TableResolver resolver)
        {
            List<FieldInfoEventArgs> list = resolver.GetFieldInfo(UpdateKind.Delete);
            if (resolver.DataAdapter.DeleteCommand == null)
                resolver.DataAdapter.DeleteCommand = resolver.Context.CreateCommand();
            else
                resolver.DataAdapter.DeleteCommand.Parameters.Clear();
            resolver.DataAdapter.DeleteCommand.CommandText = CreateDeleteSql(resolver, list);
            list = null;
        }

        private static string CreateDeleteSql(TableResolver resolver, List<FieldInfoEventArgs> list)
        {
            return string.Format(ObjectUtil.SysCulture, "DELETE FROM {0} {1}",
                resolver.Context.EscapeName(resolver.TableName),
                CreateWhereSql(resolver, resolver.DataAdapter.DeleteCommand, list, true));
        }
    }
}