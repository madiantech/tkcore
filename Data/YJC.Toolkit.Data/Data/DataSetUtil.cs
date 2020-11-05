using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public static class DataSetUtil
    {
        public static DataTable CloneTableStructure(DataTable table)
        {
            TkDebug.AssertArgumentNull(table, "table", null);

            DataTable result = new DataTable(table.TableName) { Locale = ObjectUtil.SysCulture };
            foreach (DataColumn column in table.Columns)
                result.Columns.Add(column.ColumnName, column.DataType);
            return result;
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldNames">字段序列</param>
        /// <returns>数据表</returns>
        public static DataTable CreateDataTable(string tableName, params string[] fieldNames)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);
            TkDebug.AssertEnumerableArgumentNullOrEmpty(fieldNames, "fieldNames", null);

            DataTable table = new DataTable(tableName) { Locale = ObjectUtil.SysCulture };
            foreach (string fieldName in fieldNames)
                table.Columns.Add(new DataColumn(fieldName, typeof(string)));
            return table;
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldNames">字段序列</param>
        /// <returns>数据表</returns>
        public static DataTable CreateDataTable(string tableName, IEnumerable<IFieldInfo> fieldNames)
        {
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);
            TkDebug.AssertEnumerableArgumentNull(fieldNames, "fieldNames", null);

            DataTable table = new DataTable(tableName) { Locale = ObjectUtil.SysCulture };
            foreach (IFieldInfo fieldName in fieldNames)
                table.Columns.Add(new DataColumn(fieldName.NickName,
                    MetaDataUtil.ConvertDataTypeToType(fieldName.DataType)));
            return table;
        }

        private static string GetKeyFields(DataColumn[] keys)
        {
            if (keys.Length == 1)
                return keys[0].ColumnName;
            else
                return string.Join(", ", from key in keys select key.ColumnName);
        }

        public static void SetPrimaryKey(DataTable table, params DataColumn[] keys)
        {
            TkDebug.AssertArgumentNull(table, "table", null);
            TkDebug.AssertEnumerableArgumentNull<DataColumn>(keys, "keys", null);

            try
            {
                table.PrimaryKey = keys;
            }
            catch (DataException ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "无法给表{0}设置主键，请检查表中的主键字段{1}是否存在重复数据",
                    table.TableName, GetKeyFields(keys)), ex, null);
            }
        }

        public static string GetXml(DataTable table)
        {
            TkDebug.AssertArgumentNull(table, "table", null);

            StringBuilder builder = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(builder);
            using (writer)
            {
                table.WriteXml(writer, XmlWriteMode.WriteSchema, false);
                return builder.ToString();
            }
        }

        public static DataSet Xml2DataSet(string xml)
        {
            DataSet result = new DataSet { Locale = ObjectUtil.SysCulture };
            using (StringReader tempReader = new StringReader(xml))
            {
                result.ReadXml(tempReader, XmlReadMode.Auto);
                return result;
            }
        }

        public static DataTable GetDataTable(string xml)
        {
            TkDebug.AssertArgumentNullOrEmpty(xml, "xml", null);

            TextReader reader = new StringReader(xml);
            using (reader)
            {
                DataTable table = new DataTable { Locale = ObjectUtil.SysCulture };
                table.ReadXml(reader);
                return table;
            }
        }

        internal static DataTable CopyToDataTable<T>(this IEnumerable<T> source,
            DataTable table, int topCount) where T : DataRow
        {
            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                table.BeginLoadData();
                try
                {
                    int i = 0;
                    while (enumerator.MoveNext())
                    {
                        if (topCount > 0 && i++ >= topCount)
                            break;
                        DataRow current = enumerator.Current;
                        if (current != null)
                        {
                            CopyRow(table, current);
                        }
                    }

                    return table;
                }
                finally
                {
                    table.EndLoadData();
                }
            }
        }

        internal static void CopyRow(DataTable table, DataRow row)
        {
            object[] values = null;
            try
            {
                switch (row.RowState)
                {
                    case DataRowState.Detached:
                        if (!row.HasVersion(DataRowVersion.Proposed))
                        {
                            //throw DataSetUtil.InvalidOperation(Strings.DataSetLinq_CannotLoadDetachedRow);
                        }
                        break;

                    case DataRowState.Unchanged:
                    case DataRowState.Added:
                    case DataRowState.Modified:
                        break;

                    case DataRowState.Deleted:
                        //throw DataSetUtil.InvalidOperation(Strings.DataSetLinq_CannotLoadDeletedRow);
                        break;

                    default:
                        //throw DataSetUtil.InvalidDataRowState(current.RowState);
                        break;
                }
                values = row.ItemArray;
                table.LoadDataRow(values, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 对DataRow进行赋值，注意，输入的values必须是从起始开始到结束相应的Row的ColName序列
        /// </summary>
        /// <param name="row">需要赋值的DataRow</param>
        /// <param name="values">相应的值</param>
        /// <example>例如：
        /// <code>
        /// DataSetUtil.SetRowValues(row, new object[] {"01", "先生"});
        /// </code>
        /// </example>
        public static void SetRowValues(DataRow row, params object[] values)
        {
            TkDebug.AssertArgumentNull(row, "row", null);
            TkDebug.AssertEnumerableArgumentNull(values, "values", null);

            row.BeginEdit();
            try
            {
                for (int i = 0; i < values.Length; ++i)
                    row[i] = values[i];
            }
            finally
            {
                row.EndEdit();
            }
        }

        /// <summary>
        /// 对DataRow中指定的字段进行赋值
        /// </summary>
        /// <param name="row">需要赋值的DataRow</param>
        /// <param name="names">指定的字段序列</param>
        /// <param name="values">相应的值</param>
        /// <example>例如：
        /// <code>
        /// DataSetUtil.SetRowValues(row, new string[] {"LD_NAME", "LD_DATE", "LD_USERID"},
        ///     new object[] {"Hello", DateTime.Now, Info.UserId});
        /// </code>
        /// </example>
        public static void SetRowValues(DataRow row, string[] names, params object[] values)
        {
            TkDebug.AssertArgumentNull(row, "row", null);
            TkDebug.AssertEnumerableArgumentNullOrEmpty(names, "names", null);
            TkDebug.AssertArgumentNull(values, "values", null);
            TkDebug.AssertArgument(names.Length == values.Length, "values", string.Format(ObjectUtil.SysCulture,
                "参数names的个数是{0}，参数values的个数是{1}，两者个数不匹配，请确认", names.Length, values.Length), null);

            row.BeginEdit();
            try
            {
                for (int i = 0; i < names.Length; ++i)
                    row[names[i]] = values[i] ?? DBNull.Value;
            }
            finally
            {
                row.EndEdit();
            }
        }

        public static void SetSafeValue(DataRow row, DataColumn column, string value)
        {
            SetSafeValue(row, column, value, false);
        }

        public static void SetSafeValue(DataRow row, DataColumn column, string value, bool throwOnError)
        {
            TkDebug.AssertArgumentNull(row, "row", null);
            TkDebug.AssertArgumentNull(column, "column", null);

            if (string.IsNullOrEmpty(value))
                row[column] = DBNull.Value;
            else
            {
                try
                {
                    row[column] = ObjectUtil.GetValue(null, column.DataType,
                        value, null, ObjectUtil.ReadSettings);
                }
                catch
                {
                    if (throwOnError)
                        throw;
                }
            }
        }

        public static void SetSafeValue(DataRow row, string fieldName, string value)
        {
            SetSafeValue(row, fieldName, value, false);
        }

        public static void SetSafeValue(DataRow row, string fieldName, string value, bool throwOnError)
        {
            TkDebug.AssertArgumentNull(row, "row", null);
            TkDebug.AssertArgumentNull(fieldName, "fieldName", null);

            if (string.IsNullOrEmpty(value))
                row[fieldName] = DBNull.Value;
            else
            {
                TkDebug.AssertNotNull(row.Table, "row没有放在Table下，无法获取", null);
                DataColumn column = row.Table.Columns[fieldName];
                try
                {
                    row[column] = ObjectUtil.GetValue(null, column.DataType,
                        value, null, ObjectUtil.ReadSettings);
                }
                catch
                {
                    if (throwOnError)
                        throw;
                }
            }
        }

        /// <summary>
        /// 清空DataTable的内容，使用Delete方法
        /// </summary>
        /// <param name="table">需要清空的表</param>
        public static void EmptyDataTable(DataTable table)
        {
            foreach (DataRow row in table.Rows)
                row.Delete();
        }

        /// <summary>
        /// 拷贝一条DataRow，使用每个Column的Index进行拷贝，适用于表结构完全相同的DataRow
        /// </summary>
        /// <param name="src">源DataRow</param>
        /// <param name="dst">目标DataRow</param>
        /// <param name="start">起始的Column位置</param>
        /// <param name="end">终止的Column位置</param>
        /// <example>例如：
        /// <code>
        /// DataSetCopyUtil.CopyRow(rowSource, rowDest, 0, 5);
        /// </code>
        /// rowSource与rowDest的结构完全一致，将rowSource的第一个字段到第6个字段的内容依次拷贝到rowDest中
        /// </example>
        public static void CopyRow(DataRow src, DataRow dst, int start, int end)
        {
            dst.BeginEdit();
            try
            {
                for (int i = start; i <= end; ++i)
                    dst[i] = src[i];
            }
            finally
            {
                dst.EndEdit();
            }
        }

        /// <summary>
        /// 拷贝一条DataRow，使用每个Column的Index进行拷贝，适用于表结构完全相同的DataRow
        /// </summary>
        /// <param name="src">源行</param>
        /// <param name="dst">目的行</param>
        /// <example>例如：
        /// <code>
        /// DataSetCopyUtil.CopyRow(rowSource, rowDest);
        /// </code>
        /// rowSource与rowDest的结构完全一致，将rowSource的的所有字段内容依次拷贝到rowDest中
        /// </example>
        public static void CopyRow(DataRow src, DataRow dst)
        {
            CopyRow(src, dst, 0, src.Table.Columns.Count - 1);
        }

        /// <summary>
        /// 将DataRow src中指定的字段拷贝到DataRow dst中指定的字段，
        /// 源行与目标行的字段名可以不相同，单类型必须向下兼容
        /// </summary>
        /// <param name="src">源DataRow</param>
        /// <param name="dst">目标DataRow</param>
        /// <param name="srcNames">源字段名序列</param>
        /// <param name="dstNames">目标字段名序列</param>
        /// <example>例如：
        /// <code>
        /// DataSetUtil.CopyRow(account, lead, new string[] {"ACC_NAME", "ACC_COMPANY"},
        ///     new string[] {"LD_NAME", "LD_COMPANY"});
        /// </code>
        /// </example>
        public static void CopyRow(DataRow src, DataRow dst, string[] srcNames, string[] dstNames)
        {
            dst.BeginEdit();
            try
            {
                for (int i = 0; i < dstNames.Length; ++i)
                    dst[dstNames[i]] = src[srcNames[i]];
            }
            finally
            {
                dst.EndEdit();
            }
        }

        /// <summary>
        /// 拷贝一条DataRow，使用每个Column的Name进行拷贝，适用于字段名完全相同，但字段顺序不一致的表
        /// </summary>
        /// <param name="src">源DataRow</param>
        /// <param name="dst">目标DataRow</param>
        /// <param name="start">起始的Column位置</param>
        /// <param name="end">终止的Column位置</param>
        public static void CopyRowByName(DataRow src, DataRow dst, int start, int end)
        {
            DataColumnCollection cols = src.Table.Columns;
            dst.BeginEdit();
            try
            {
                for (int i = start; i <= end; ++i)
                {
                    DataColumn col = cols[i];
                    dst[col.ColumnName] = src[col];
                }
            }
            finally
            {
                dst.EndEdit();
            }
        }

        /// <summary>
        /// 拷贝一条DataRow，使用每个Column的Name进行拷贝，适用于字段名完全相同，但字段顺序不一致的表
        /// </summary>
        /// <param name="src">源DataRow</param>
        /// <param name="dst">目标DataRow</param>
        public static void CopyRowByName(DataRow src, DataRow dst)
        {
            CopyRowByName(src, dst, 0, dst.Table.Columns.Count - 1);
        }

        /// <summary>
        /// 拷贝DataTable，使用CopyRow方法复制每个Row
        /// </summary>
        /// <param name="src">源DataTable</param>
        /// <param name="dst">目标DataTable</param>
        public static void CopyDataTable(DataTable src, DataTable dst)
        {
            EmptyDataTable(dst);
            AppendDataTable(src, dst);
        }

        /// <summary>
        /// 向与源表结构完全相同的目的表拷贝数据
        /// </summary>
        /// <param name="src">源表</param>
        /// <param name="dst">目的表</param>
        public static void AppendDataTable(DataTable src, DataTable dst)
        {
            foreach (DataRow row in src.Rows)
            {
                DataRow newRow = dst.NewRow();
                CopyRow(row, newRow);
                dst.Rows.Add(newRow);
            }
        }

        /// <summary>
        /// 拷贝DataTable，使用CopyRowByName方法复制每个Row
        /// </summary>
        /// <param name="src">源DataTable</param>
        /// <param name="dst">目标DataTable</param>
        public static void CopyDataTableByName(DataTable src, DataTable dst)
        {
            EmptyDataTable(dst);
            AppendDataTableByName(src, dst);
        }

        /// <summary>
        /// 从源表向目的表拷贝数据，源表和目的表的字段名都一致，但顺序可能不一样
        /// </summary>
        /// <param name="src">源表</param>
        /// <param name="dst">目的表</param>
        public static void AppendDataTableByName(DataTable src, DataTable dst)
        {
            foreach (DataRow row in src.Rows)
            {
                DataRow newRow = dst.NewRow();
                CopyRowByName(row, newRow);
                dst.Rows.Add(newRow);
            }
        }

        /// <summary>
        /// 只拷贝表的部分字段到另外一个表，注意，这里要求两个表的字段名完全一样。拷贝前，先清空dst表
        /// </summary>
        /// <param name="src">源DataTable</param>
        /// <param name="dst">目标DataTable</param>
        /// <param name="fieldNames">需要拷贝的字段名称</param>
        public static void CopyPartDataTable(DataTable src, DataTable dst, params string[] fieldNames)
        {
            EmptyDataTable(dst);
            foreach (DataRow row in src.Rows)
            {
                DataRow newRow = dst.NewRow();
                newRow.BeginEdit();
                try
                {
                    foreach (string fieldName in fieldNames)
                        newRow[fieldName] = row[fieldName];
                }
                finally
                {
                    newRow.EndEdit();
                }
                dst.Rows.Add(newRow);
            }
        }

        /// <summary>
        /// 只拷贝表的部分字段到另外一个表，注意拷贝前，先清空dst表
        /// </summary>
        /// <param name="src">源DataTable</param>
        /// <param name="dst">目标DataTable</param>
        /// <param name="srcNames">需要拷贝的字段名称</param>
        /// <param name="dstNames">目标表对应的字段名称</param>
        /// <example> 例如：
        /// <code>
        /// DataSetUtil.CopyPartDataTable(account, lead, new string[] {"ACC_NAME", "ACC_COMPANY"},
        ///     new string[] {"LD_NAME", "LD_COMPANY"});
        /// </code>
        /// </example>
        public static void CopyPartDataTable(DataTable src, DataTable dst, string[] srcNames, string[] dstNames)
        {
            EmptyDataTable(dst);
            foreach (DataRow row in src.Rows)
            {
                DataRow newRow = dst.NewRow();
                CopyRow(row, newRow, srcNames, dstNames);
                dst.Rows.Add(newRow);
            }
        }

        public static DataRow GetPostRow(IInputData input, string tableName)
        {
            TkDebug.AssertArgumentNull(input, "input", null);
            TkDebug.AssertArgumentNullOrEmpty(tableName, "tableName", null);

            DataSet postDataSet = input.PostObject.Convert<DataSet>();
            DataTable table = postDataSet.Tables[tableName];
            TkDebug.AssertNotNull(table, string.Format(ObjectUtil.SysCulture,
                "PostDataSet中没有表{0}", tableName), null);
            TkDebug.Assert(table.Rows.Count > 0, string.Format(ObjectUtil.SysCulture,
                "Post表{0}中的没有数据", tableName), null);

            return table.Rows[0];
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability",
            "CA1502:AvoidExcessiveComplexity")]
        public static TkDataType ConvertDbTypeToTkDataType(DbType type)
        {
            switch (type)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    return TkDataType.String;

                case DbType.Binary:
                    return TkDataType.Binary;

                case DbType.Boolean:
                    return TkDataType.Bit;

                case DbType.Byte:
                case DbType.SByte:
                    return TkDataType.Byte;

                case DbType.Currency:
                    return TkDataType.Money;

                case DbType.Date:
                    return TkDataType.Date;

                case DbType.DateTime:
                case DbType.Time:
                    return TkDataType.DateTime;

                case DbType.Decimal:
                case DbType.VarNumeric:
                    return TkDataType.Decimal;

                case DbType.Double:
                case DbType.Single:
                    return TkDataType.Double;

                case DbType.Guid:
                    return TkDataType.Guid;

                case DbType.Int16:
                case DbType.UInt16:
                    return TkDataType.Short;

                case DbType.Int32:
                case DbType.UInt32:
                    return TkDataType.Int;

                case DbType.Int64:
                case DbType.UInt64:
                    return TkDataType.Long;

                case DbType.Xml:
                    return TkDataType.Xml;

                default:
                    return TkDataType.String;
            }
        }

        public static DbType ConvertTkDataTypeToDbType(TkDataType type)
        {
            DbType result = DbType.AnsiString;

            switch (type)
            {
                case TkDataType.String:
                case TkDataType.Text:
                    result = DbType.AnsiString;
                    break;

                case TkDataType.Int:
                    result = DbType.Int32;
                    break;

                case TkDataType.Date:
                    result = DbType.Date;
                    break;

                case TkDataType.DateTime:
                    result = DbType.DateTime;
                    break;

                case TkDataType.Double:
                    result = DbType.Double;
                    break;

                case TkDataType.Money:
                    result = DbType.Currency;
                    break;

                case TkDataType.Blob:
                case TkDataType.Binary:
                    result = DbType.Binary;
                    break;

                case TkDataType.Guid:
                    result = DbType.Guid;
                    break;

                case TkDataType.Xml:
                    result = DbType.AnsiString;
                    break;

                case TkDataType.Bit:
                    result = DbType.Boolean;
                    break;

                case TkDataType.Byte:
                    result = DbType.Byte;
                    break;

                case TkDataType.Short:
                    result = DbType.Int16;
                    break;

                case TkDataType.Long:
                    result = DbType.Int64;
                    break;

                case TkDataType.Decimal:
                    result = DbType.Decimal;
                    break;
            }
            return result;
        }
    }
}