using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [TkTypeConverter(typeof(DbContextConfigConverter))]
    public sealed class DbContextConfig : IRegName
    {
        private IDbProvider fDbProvider;
        private ISqlProvider fSqlProvider;

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return Name;
            }
        }

        #endregion

        internal IDbProvider InternalDbProvider
        {
            get
            {
                if (fDbProvider == null)
                    fDbProvider = PlugInFactoryManager.CreateInstance<IDbProvider>(
                        DbProviderPlugInFactory.REG_NAME, DbProvider);
                return fDbProvider;
            }
        }

        internal ISqlProvider InternalSqlProvider
        {
            get
            {
                if (fSqlProvider == null)
                    fSqlProvider = PlugInFactoryManager.CreateInstance<ISqlProvider>(
                        SqlProviderPlugInFactory.REG_NAME, SqlProvider);
                return fSqlProvider;
            }
        }

        [SimpleAttribute]
        public string Name { get; private set; }

        [SimpleAttribute]
        public string DbProvider { get; private set; }

        [SimpleAttribute]
        public string SqlProvider { get; private set; }

        [SimpleAttribute]
        public string ConnectionString { get; private set; }

        [SimpleAttribute(DefaultValue = 1)]
        public int IdStep { get; private set; }

        [SimpleAttribute]
        public bool Encode { get; private set; }

        [SimpleAttribute]
        public bool Default { get; private set; }

        [SimpleAttribute]
        public string ProviderName { get; private set; }

        public IDbConnection CreateConnection()
        {
            IDbConnection connection = InternalDbProvider.CreateConnection();
            connection.ConnectionString = ConnectionString;
            return connection;
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            return InternalDbProvider.CreateDataAdapter();
        }

        public IDbCommand CreateCommand()
        {
            return InternalDbProvider.CreateCommand();
        }

        public IDbCommand CreateCommand(IDbConnection connection)
        {
            TkDebug.AssertArgumentNull(connection, "connection", this);

            IDbCommand command = InternalDbProvider.CreateCommand();
            command.Connection = connection;
            return command;
        }

        public IDbDataParameter CreateParameter(TkDataType dataType)
        {
            return InternalDbProvider.CreateParameter(dataType);
        }

        public string GetSqlParamName(string fieldName, bool isOrigin)
        {
            return InternalDbProvider.GetSqlParamName(fieldName, isOrigin);
        }

        public string GetParamName(string fieldName, bool isOrigin)
        {
            return InternalDbProvider.GetParamName(fieldName, isOrigin);
        }

        public IListSqlContext GetListSql(string selectFields, string tableName, IFieldInfo[] keyFields,
            string whereClause, string orderBy, int startNum, int endNum)
        {
            return InternalSqlProvider.GetListSql(selectFields, tableName, keyFields,
                whereClause, orderBy, startNum, endNum);
        }

        public string GetFunction(string funcName, params object[] funcParams)
        {
            return InternalSqlProvider.GetFunction(funcName, funcParams);
        }

        public string GetUniId(string name, TkDbContext context)
        {
            return InternalSqlProvider.GetUniId(name, context);
        }

        public void SetListData(IListSqlContext context, ISimpleAdapter adapter, DataSet dataSet,
            int startRecord, int maxRecords, string srcTable)
        {
            InternalSqlProvider.SetListData(context, adapter, dataSet, startRecord, maxRecords, srcTable);
        }

        public string EscapeName(string name)
        {
            return InternalSqlProvider.EscapeName(name);
        }

        public TkDbContext CreateDbContext()
        {
            return new TkDbContext(this);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? string.Empty : Name;
        }

        public static DbContextConfig Create(string sqlProvider,
            string dbProvider, string connectionString)
        {
            return Create(null, sqlProvider, dbProvider, connectionString);
        }

        public static DbContextConfig Create(string name, string sqlProvider,
            string dbProvider, string connectionString)
        {
            TkDebug.AssertArgumentNullOrEmpty(sqlProvider, "sqlProvider", null);
            TkDebug.AssertArgumentNullOrEmpty(dbProvider, "dbProvider", null);
            TkDebug.AssertArgumentNullOrEmpty(connectionString, "connectionString", null);

            TkDebug.ThrowIfNoGlobalVariable();

            DbContextConfig config = new DbContextConfig
            {
                Name = name,
                SqlProvider = sqlProvider,
                DbProvider = dbProvider,
                ConnectionString = connectionString
            };
            return config;
        }
    }
}
