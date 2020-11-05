using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    /// <remarks>�ӿ�<c>ISQLProvider</c>��ToolKit��SQL����ṩ���ӿڡ�ToolKit����ڿ���ASP.NET���򣬳��������SQL���淶��
    /// ���б��ҳ�������½����ݵ�ȫ��ID�ȣ���ͬ���ݿ⣨����Դ���Դ��ɲ�ͬ��ʵ�ַ�ʽ������Ҫ��ɸýӿ�</remarks>
    /// <summary>SQL����ṩ���ӿ�</summary>
    [TkTypeConverter(typeof(SqlProviderTypeConverter))]
    public interface ISqlProvider
    {
        /// <summary>
        /// ���Listҳ��������ݵ�SQL��� 
        /// </summary>
        /// <param name="selectFields">�ֶμ��ϣ�ͨ��", "���зָ�</param>
        /// <param name="tableName">����</param>
        /// <param name="keyFields">�����ֶμ��ϣ�ͨ��", "���зָ�</param>
        /// <param name="keyType">�����ֶ�����</param>
        /// <param name="whereClause">where���</param>
        /// <param name="orderBy">order by���</param>
        /// <param name="startNum">��ʼ����ֵ</param>
        /// <param name="endNum">��������ֵ</param>
        IListSqlContext GetListSql(string selectFields, string tableName, IFieldInfo[] keyFields,
            string whereClause, string orderBy, int startNum, int endNum);

        string GetRowNumSql(string tableName, IFieldInfo[] keyFields, string whereClause,
            string rowNumFilter, string orderBy, int startNum, int endNum);

        /// <summary>
        /// ������ݿ⣨����Դ��SQL����
        /// </summary>
        /// <param name="funcName">����������</param>
        /// <param name="funcParams">��������</param>
        /// <returns>�����ݿ�Ϸ��ĺ���</returns>
        string GetFunction(string funcName, params object[] funcParams);

        /// <summary>
        /// ���ȫ��ΨһId
        /// </summary>
        /// <param name="name">��Ҫ���Id�����ƣ�һ���ڿ����Ϊ����ı���</param>
        /// <param name="connection">���ݿ�����</param>
        /// <returns>�����Ƶ���һ��ֵ</returns>
        string GetUniId(string name, TkDbContext context);
        /// <summary>
        /// ����Listҳ������
        /// </summary>
        /// <param name="adapter">���ݿ�������</param>
        /// <param name="dataSet">���ݼ�</param>
        /// <param name="startRecord">��ʼ��¼</param>
        /// <param name="maxRecords">����¼��</param>
        /// <param name="srcTable">����Դ��</param>
        void SetListData(IListSqlContext context, ISimpleAdapter adapter, DataSet dataSet,
            int startRecord, int maxRecords, string srcTable);

        string GetExistTableSql(string tableName);

        string GetCreateTableSql(ITableSchemeEx scheme);

        string GetDropTableSql(string tableName);

        string EscapeName(string name);
    }
}
