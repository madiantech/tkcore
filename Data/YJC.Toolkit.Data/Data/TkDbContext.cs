using System;
using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class TkDbContext : IDisposable
    {
        public static readonly TkDbContext EmptyContext = new TkDbContext(null, null);

        private readonly DbContextConfig fContextConfig;
        private readonly bool fCreateConnection;

        internal TkDbContext(DbContextConfig contextConfig)
        {
            fContextConfig = contextConfig;
            DbConnection = contextConfig.CreateConnection();
            fCreateConnection = true;
        }

        internal TkDbContext(DbContextConfig contextConfig, IDbConnection connection)
        {
            fContextConfig = contextConfig;
            DbConnection = connection;
            fCreateConnection = false;
        }

        public IDbConnection DbConnection { get; private set; }

        public DbContextConfig ContextConfig
        {
            get
            {
                if (this == EmptyContext)
                    TkDebug.ThrowToolkitException("EmptyContext无数据操作能力", this);
                TkDebug.AssertNotNull(fContextConfig, "ContextConfig没有设置", this);

                return fContextConfig;
            }
        }

        public IDbCommand CreateCommand()
        {
            return ContextConfig.CreateCommand(DbConnection);
        }

        public IDbDataParameter CreateParameter(TkDataType dataType)
        {
            return ContextConfig.CreateParameter(dataType);
        }

        public IDbDataParameter CreateParameter(IFieldInfo field)
        {
            return CreateParameter(field, false);
        }

        public IDbDataParameter CreateParameter(IFieldInfo field, bool isOrigin)
        {
            IDbDataParameter result = CreateParameter(field.DataType);
            result.ParameterName = GetParamName(field.FieldName, isOrigin);
            return result;
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            return ContextConfig.CreateDataAdapter();
        }

        public string GetUniId(string name)
        {
            return ContextConfig.GetUniId(name, this);
        }

        public string GetSqlParamName(string fieldName, bool isOrigin)
        {
            return ContextConfig.GetSqlParamName(fieldName, isOrigin);
        }

        public string GetSqlParamName(string fieldName)
        {
            return GetSqlParamName(fieldName, false);
        }

        public string GetParamName(string fieldName, bool isOrigin)
        {
            return ContextConfig.GetParamName(fieldName, isOrigin);
        }

        public string GetParamName(string fieldName)
        {
            return GetParamName(fieldName, false);
        }

        public string EscapeName(string name)
        {
            return ContextConfig.EscapeName(name);
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (this == EmptyContext)
                return;
            if (fCreateConnection)
                DbConnection.DisposeObject();
        }

        #endregion
    }
}
