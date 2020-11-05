using System;
using System.Data;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public static partial class DbUtil
    {
        public static object ExecuteScalar(string sql, IDbConnection connection)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", null);
            TkDebug.AssertArgumentNull(connection, "connection", null);

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                return ExecuteScalar(connection, command);
            }
        }

        public static object ExecuteScalar(string sql, TkDbContext context)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", null);
            TkDebug.AssertArgumentNull(context, "context", null);

            using (IDbCommand command = context.CreateCommand())
            {
                command.CommandText = sql;
                return ExecuteScalar(context.DbConnection, command);
            }
        }

        private static object ExecuteScalar(IDbConnection connection, IDbCommand command)
        {
            DbConnectionStatus item = new DbConnectionStatus(connection);
            object result = null;
            item.OpenDbConnection();
            try
            {
                result = command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "执行SQL:{0}出错", command.CommandText), ex, null);
            }
            finally
            {
                item.CloseDbConnection();
            }
            if (result == null)
                result = DBNull.Value;
            return result;
        }

        public static object ExecuteScalar(string sql, IDbConnection connection,
            params IDbDataParameter[] parameters)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", null);
            TkDebug.AssertArgumentNull(connection, "connection", null);
            TkDebug.AssertArgumentNull(parameters, "parameters", null);

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                return ExecuteScalar(connection, command, parameters);
            }
        }

        public static object ExecuteScalar(string sql, TkDbContext context,
            params IDbDataParameter[] parameters)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", null);
            TkDebug.AssertArgumentNull(context, "context", null);
            TkDebug.AssertArgumentNull(parameters, "parameters", null);

            using (IDbCommand command = context.CreateCommand())
            {
                command.CommandText = sql;
                return ExecuteScalar(context.DbConnection, command, parameters);
            }
        }

        public static object ExecuteScalar(string sql, TkDbContext context,
            DbParameterList parameterlist)
        {
            TkDebug.AssertArgumentNull(parameterlist, "parameterlist", null);

            return ExecuteScalar(sql, context, parameterlist.CreateParameters(context));
        }

        public static object ExecuteScalar(string headSql, TkDbContext context, IParamBuilder builder)
        {
            TkDebug.AssertArgumentNullOrEmpty(headSql, "headSql", null);
            TkDebug.AssertArgumentNull(builder, "builder", null);

            string whereSql = builder.Sql;
            if (string.IsNullOrEmpty(whereSql))
                return ExecuteScalar(headSql, context);
            else
            {
                string sql = headSql + " WHERE " + whereSql;
                return ExecuteScalar(sql, context, builder.Parameters);
            }
        }

        private static object ExecuteScalar(IDbConnection connection, IDbCommand command,
            IDbDataParameter[] parameters)
        {
            SetParameter(command, parameters);
            return ExecuteScalar(connection, command);
        }

        public static int ExecuteNonQuery(string sql, IDbConnection connection)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", null);
            TkDebug.AssertArgumentNull(connection, "connection", null);

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                return ExecuteNonQuery(connection, command);
            }
        }

        public static int ExecuteNonQuery(string sql, TkDbContext context)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", null);
            TkDebug.AssertArgumentNull(context, "context", null);

            using (IDbCommand command = context.CreateCommand())
            {
                command.CommandText = sql;
                return ExecuteNonQuery(context.DbConnection, command);
            }
        }

        private static int ExecuteNonQuery(IDbConnection connection, IDbCommand command)
        {
            DbConnectionStatus item = new DbConnectionStatus(connection);
            item.OpenDbConnection();
            try
            {
                int result = command.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "执行SQL:{0}出错", command.CommandText), ex, null);
                return -1;
            }
            finally
            {
                item.CloseDbConnection();
            }
        }

        public static int ExecuteNonQuery(string sql, IDbConnection connection,
            params IDbDataParameter[] parameters)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", null);
            TkDebug.AssertArgumentNull(connection, "connection", null);
            TkDebug.AssertEnumerableArgumentNull<IDbDataParameter>(parameters, "parameters", null);

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                return ExecuteNonQuery(connection, command, parameters);
            }
        }

        public static int ExecuteNonQuery(string sql, TkDbContext context,
            params IDbDataParameter[] parameters)
        {
            TkDebug.AssertArgumentNullOrEmpty(sql, "sql", null);
            TkDebug.AssertArgumentNull(context, "context", null);
            TkDebug.AssertEnumerableArgumentNull<IDbDataParameter>(parameters, "parameters", null);

            using (IDbCommand command = context.CreateCommand())
            {
                command.CommandText = sql;
                return ExecuteNonQuery(context.DbConnection, command, parameters);
            }
        }

        public static int ExecuteNonQuery(string sql, TkDbContext context,
            DbParameterList parameterList)
        {
            TkDebug.AssertArgumentNull(parameterList, "parameterList", null);

            return ExecuteNonQuery(sql, context, parameterList.CreateParameters(context));
        }

        public static int ExecuteNonQuery(string headSql, TkDbContext context, IParamBuilder builder)
        {
            TkDebug.AssertArgumentNullOrEmpty(headSql, "headSql", null);
            TkDebug.AssertArgumentNull(builder, "builder", null);

            string whereSql = builder.Sql;
            if (string.IsNullOrEmpty(whereSql))
                return ExecuteNonQuery(headSql, context);
            else
            {
                string sql = headSql + " WHERE " + whereSql;
                return ExecuteNonQuery(sql, context, builder.Parameters);
            }
        }

        private static int ExecuteNonQuery(IDbConnection connection, IDbCommand command,
            params IDbDataParameter[] parameters)
        {
            SetParameter(command, parameters);
            return ExecuteNonQuery(connection, command);
        }

        //public static StoredProc CreateStoredProc(string xmlFile)
        //{
        //    StoredProcXml xml = CacheManager.GetItem(xmlFile, StoredProcCacheCreator.Creator);
        //    StoredProc proc = new XmlStoredProc(xml.StoredProc);
        //    return proc;
        //}

        public static StoredProc CreateStoredProc(Stream stream)
        {
            StoredProcXml xml = new StoredProcXml();
            xml.ReadFromStream("Xml", null, stream, ObjectUtil.ReadSettings, QName.Toolkit);
            //xml.LoadFromReader(XmlUtil.GetXmlReader(stream));
            StoredProc proc = new XmlStoredProc(xml.StoredProc);
            return proc;
        }

        private static void SetParameter(IDbCommand command, IDbDataParameter[] parameters)
        {
            Array.ForEach(parameters, param => command.Parameters.Add(param));
        }
    }
}
