using System;
using System.Runtime.CompilerServices;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    /// <remarks>Class<c>SerialId</c>���ó���ģ������Id���������ʹ�ã�����Ч�ʲ��ߣ����ʺ�С�����ݿ�ʹ�á�
    /// ���SqlServer���ô洢���̣����Oracle����Sequence��������߶����ṩȫ��ID�ĸ�Ч��ȡ��ʽ��
    /// �����Access֮������ݿ⣬ȫ��Id�������ֹ��������ܷ��࣬���ܱ��̳�
    /// </remarks>
    /// <summary>�ó���ģ������Id</summary>
    internal static class SerialId
    {
        /// <summary>
        /// �õ�ĳ�������µ���һ���Ϸ�����ֵ��
        /// </summary>
        /// <param name="name">��ҪȡID������</param>
        /// <param name="connection">���ݿ�����</param>		
        /// <returns>�����Ƶ���һ��ֵ</returns>
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
