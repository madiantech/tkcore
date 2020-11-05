using System;
using System.Data;

namespace YJC.Toolkit.Sys
{
    [EvaluateAddition(Author = "YJC", CreateDate = "2018-04-16",
        Description = "添加DataSetExtension类型扩展")]
    public static class DataSetExtension
    {
        public static DataRow GetRow(this DataSet dataSet, string tableName, int position)
        {
            try
            {
                DataTable table = dataSet.Tables[tableName];
                DataRow row = table?.Rows[position];
                return row;
            }
            catch
            {
                return null;
            }
        }

        public static DataRow GetRow(this DataSet dataSet, string tableName)
        {
            return GetRow(dataSet, tableName, 0);
        }

        public static object GetFieldValue(this DataSet dataSet, string tableName, string fieldName)
        {
            try
            {
                DataRow row = GetRow(dataSet, tableName);
                return row[fieldName];
            }
            catch
            {
                return DBNull.Value;
            }
        }

        public static T GetFieldValue<T>(this DataSet dataSet, string tableName, string fieldName)
        {
            object value = GetFieldValue(dataSet, tableName, fieldName);
            return value.Value<T>();
        }

        public static string GetDateTime(this DataRow row, string fieldName, string format)
        {
            try
            {
                object value = row[fieldName];
                if (value == DBNull.Value)
                    return string.Empty;
                return value.Value<DateTime>().ToString(format, ObjectUtil.SysCulture);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T GetValue<T>(this DataRow row, string fieldName)
        {
            try
            {
                return row[fieldName].Value<T>();
            }
            catch
            {
                return default(T);
            }
        }

        public static string GetString(this DataRow row, string fieldName)
        {
            try
            {
                return row[fieldName].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}