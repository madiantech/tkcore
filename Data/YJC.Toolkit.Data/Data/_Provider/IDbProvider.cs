using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    /// <remarks>�ӿ�<c>IDbProvider</c>�����ݿ�����ṩ�߽ӿڡ�ADO.NET�����ݿ��ṩ�ߣ�Provider������Ϊһ���߼��ϵķ��飬
    /// ����Command��Connection��DataReader��DataAdapter�ȸ��ToolKit��ͬ��ADO.NET�����������ݿ����Щ����ӿ��ṩ��װ��
    /// ������ͬ���͵����ݿⶼ��Ҫ��ɸýӿڡ�</remarks>
    /// <summary>���ݿ�����ṩ�߽ӿ�</summary>
    [TkTypeConverter(typeof(DbProviderTypeConverter))]
    public interface IDbProvider
    {
        IDbConnection CreateConnection();

        IDbDataAdapter CreateDataAdapter();

        IDbCommand CreateCommand();

        IDbDataParameter CreateParameter(TkDataType type);

        /// <summary>
        /// ���SQL����еĲ�����
        /// </summary>
        /// <param name="fieldName">�ֶ�����</param>
        /// <param name="isOrigin">�ǲ����µ�ֵ�����Ǿɵ�ֵ</param>
        /// <returns>������</returns>
        /// <remarks>SQL����е�Where�־䣬������ֵ��Ҫʹ���ֶεľ�ֵ�������ط����õ�ǰ��ֵ</remarks>
        string GetSqlParamName(string fieldName, bool isOrigin);
        /// <summary>
        /// ������ݿ������������Ĳ�������
        /// </summary>
        /// <param name="fieldName">�ֶ�����</param>
        /// <param name="isOrigin">�ǲ����µ�ֵ�����Ǿɵ�ֵ</param>
        /// <returns>������</returns>
        /// <remarks>SQL����е�Where�־䣬������ֵ��Ҫʹ���ֶεľ�ֵ�������ط����õ�ǰ��ֵ</remarks>
        string GetParamName(string fieldName, bool isOrigin);
    }
}
