using System;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    /// <remarks>Class<c>Sequence</c>：Oracle的Sequence封装。密封类，不能被继承
    /// </remarks>
    /// <summary>Oracle的Sequence封装</summary>
    internal static class Sequence
    {
        private static string ExecuteSequence(string name, IDbConnection connection)
        {
            string sql = string.Format(ObjectUtil.SysCulture,
                "SELECT S_{0}.nextval FROM dual", name);
            object value = DbUtil.ExecuteScalar(sql, connection);
            return value.ToString();
        }

        /// <summary>
        /// 计算Sequence的下一个值
        /// </summary>
        /// <param name="name">Sequence的名称</param>
        /// <param name="connection">数据库连接</param>
        /// <returns>该Sequence的下一个数值</returns>
        public static string Execute(string name, IDbConnection connection)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", null);
            TkDebug.AssertArgumentNull(connection, "connection", null);

            try
            {
                return ExecuteSequence(name, connection);
            }
            catch (Exception ex)
            {
                string sql = string.Format(ObjectUtil.SysCulture,
                    "SELECT COUNT(*) FROM USER_SEQUENCES WHERE SEQUENCE_NAME = 'S_{0}'", name);
                int count = int.Parse(DbUtil.ExecuteScalar(sql, connection).ToString(),
                    ObjectUtil.SysCulture);
                if (count == 0)
                {
                    sql = string.Format(ObjectUtil.SysCulture,
                        "CREATE SEQUENCE S_{0}", name);
                    DbUtil.ExecuteNonQuery(sql, connection);
                    return ExecuteSequence(name, connection);
                }
                else
                {
                    TkDebug.ThrowToolkitException(
                        string.Format(ObjectUtil.SysCulture,
                        "系统中存在S_{0}的Sequence，却无法取值，请检查数据库方面的原因", name), ex, null);
                    return null;
                }
            }
        }
    }
}
