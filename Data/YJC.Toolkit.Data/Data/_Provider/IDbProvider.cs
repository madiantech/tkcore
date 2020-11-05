using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    /// <remarks>接口<c>IDbProvider</c>：数据库操作提供者接口。ADO.NET中数据库提供者（Provider）是作为一种逻辑上的分组，
    /// 包括Command、Connection、DataReader和DataAdapter等概念。ToolKit不同与ADO.NET，将处理数据库的这些对象接口提供封装，
    /// 操作不同类型的数据库都需要完成该接口。</remarks>
    /// <summary>数据库操作提供者接口</summary>
    [TkTypeConverter(typeof(DbProviderTypeConverter))]
    public interface IDbProvider
    {
        IDbConnection CreateConnection();

        IDbDataAdapter CreateDataAdapter();

        IDbCommand CreateCommand();

        IDbDataParameter CreateParameter(TkDataType type);

        /// <summary>
        /// 获得SQL语句中的参数名
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="isOrigin">是采用新的值，还是旧的值</param>
        /// <returns>参数名</returns>
        /// <remarks>SQL语句中的Where字句，参数的值需要使用字段的旧值，其它地方采用当前新值</remarks>
        string GetSqlParamName(string fieldName, bool isOrigin);
        /// <summary>
        /// 获得数据库命令参数对象的参数名称
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="isOrigin">是采用新的值，还是旧的值</param>
        /// <returns>参数名</returns>
        /// <remarks>SQL语句中的Where字句，参数的值需要使用字段的旧值，其它地方采用当前新值</remarks>
        string GetParamName(string fieldName, bool isOrigin);
    }
}
