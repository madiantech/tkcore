using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    /// <remarks>接口<c>ISQLProvider</c>：ToolKit中SQL语句提供器接口。ToolKit框架内开发ASP.NET程序，抽象出若干SQL语句规范，
    /// 如列表分页技术，新建数据的全局ID等，不同数据库（数据源）对此由不同的实现方式，则需要完成该接口</remarks>
    /// <summary>SQL语句提供器接口</summary>
    [TkTypeConverter(typeof(SqlProviderTypeConverter))]
    public interface ISqlProvider
    {
        /// <summary>
        /// 获得List页面填充数据的SQL语句 
        /// </summary>
        /// <param name="selectFields">字段集合，通过", "进行分隔</param>
        /// <param name="tableName">表名</param>
        /// <param name="keyFields">主键字段集合，通过", "进行分隔</param>
        /// <param name="keyType">主键字段类型</param>
        /// <param name="whereClause">where语句</param>
        /// <param name="orderBy">order by语句</param>
        /// <param name="startNum">开始的数值</param>
        /// <param name="endNum">结束的数值</param>
        IListSqlContext GetListSql(string selectFields, string tableName, IFieldInfo[] keyFields,
            string whereClause, string orderBy, int startNum, int endNum);

        string GetRowNumSql(string tableName, IFieldInfo[] keyFields, string whereClause,
            string rowNumFilter, string orderBy, int startNum, int endNum);

        /// <summary>
        /// 获得数据库（数据源）SQL函数
        /// </summary>
        /// <param name="funcName">函数名代码</param>
        /// <param name="funcParams">函数参数</param>
        /// <returns>该数据库合法的函数</returns>
        string GetFunction(string funcName, params object[] funcParams);

        /// <summary>
        /// 获得全局唯一Id
        /// </summary>
        /// <param name="name">需要获得Id的名称，一般在框架中为具体的表名</param>
        /// <param name="connection">数据库连接</param>
        /// <returns>该名称的下一个值</returns>
        string GetUniId(string name, TkDbContext context);
        /// <summary>
        /// 设置List页面数据
        /// </summary>
        /// <param name="adapter">数据库适配器</param>
        /// <param name="dataSet">数据集</param>
        /// <param name="startRecord">开始记录</param>
        /// <param name="maxRecords">最大记录数</param>
        /// <param name="srcTable">数据源表</param>
        void SetListData(IListSqlContext context, ISimpleAdapter adapter, DataSet dataSet,
            int startRecord, int maxRecords, string srcTable);

        string GetExistTableSql(string tableName);

        string GetCreateTableSql(ITableSchemeEx scheme);

        string GetDropTableSql(string tableName);

        string EscapeName(string name);
    }
}
