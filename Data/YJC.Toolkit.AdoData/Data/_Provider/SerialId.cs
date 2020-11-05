using System;
using System.Runtime.CompilerServices;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    /// <remarks>Class<c>SerialId</c>：用程序模拟生成Id，该类可以使用，但是效率不高，仅适合小型数据库使用。
    /// 针对SqlServer采用存储过程，针对Oracle采用Sequence，因此两者都能提供全局ID的高效获取方式，
    /// 而针对Access之类的数据库，全局Id，必须手工产生。密封类，不能被继承
    /// </remarks>
    /// <summary>用程序模拟生成Id</summary>
    internal static class SerialId
    {
        /// <summary>
        /// 得到某个名称下的下一个合法的数值。
        /// </summary>
        /// <param name="name">需要取ID的名称</param>
        /// <param name="connection">数据库连接</param>		
        /// <returns>该名称的下一个值</returns>
        /// 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string Execute(string name, TkDbContext context)
        {
            object value;
            string sqlRead = string.Format(ObjectUtil.SysCulture,
                "SELECT ID_VALUE FROM ID_CODEID WHERE ID_NAME = '{0}'", name);
            string sqlUpdate = string.Format(ObjectUtil.SysCulture,
                "UPDATE ID_CODEID SET ID_VALUE = ID_VALUE + 1 WHERE ID_NAME = '{0}'", name);
            string sqlInsert = string.Format(ObjectUtil.SysCulture,
                "INSERT INTO ID_CODEID (ID_NAME, ID_VALUE) VALUES ('{0}', 1)", name);

            value = DbUtil.ExecuteScalar(sqlRead, context);
            if (value == DBNull.Value)
            {
                DbUtil.ExecuteNonQuery(sqlInsert, context);
                value = 0;
            }
            else
                DbUtil.ExecuteNonQuery(sqlUpdate, context);
            int num = int.Parse(value.ToString(), ObjectUtil.SysCulture) + 1;
            return num.ToString(ObjectUtil.SysCulture);
        }
    }
}
