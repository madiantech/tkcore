using System;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    /// <remarks>Class<c>Sequence</c>��Oracle��Sequence��װ���ܷ��࣬���ܱ��̳�
    /// </remarks>
    /// <summary>Oracle��Sequence��װ</summary>
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
        /// ����Sequence����һ��ֵ
        /// </summary>
        /// <param name="name">Sequence������</param>
        /// <param name="connection">���ݿ�����</param>
        /// <returns>��Sequence����һ����ֵ</returns>
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
                        "ϵͳ�д���S_{0}��Sequence��ȴ�޷�ȡֵ���������ݿⷽ���ԭ��", name), ex, null);
                    return null;
                }
            }
        }
    }
}
