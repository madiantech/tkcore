using System.Data;
using System.Data.SqlClient;
using YJC.Toolkit.Cache;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CacheInstance]
    [AlwaysCache]
    [DbProvider(RegName = REG_NAME, Author = "YJC", CreateDate = "2009-04-09", Description = DESCRIPTION)]
    internal sealed class SqlClientDbProvider : IDbProvider
    {
        internal const string REG_NAME = "SQL Server";
        internal const string DESCRIPTION = "SqlClient数据库连接提供者";

        #region IDbProvider 成员

        IDbConnection IDbProvider.CreateConnection()
        {
            return new SqlConnection();
        }

        IDbDataAdapter IDbProvider.CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }

        IDbCommand IDbProvider.CreateCommand()
        {
            SqlCommand command = new SqlCommand();
            if (BaseAppSetting.Current != null)
                command.CommandTimeout = BaseAppSetting.Current.CommandTimeout;
            return command;
        }

        IDbDataParameter IDbProvider.CreateParameter(TkDataType type)
        {
            SqlParameter param = new SqlParameter();
            SqlDbType dbType = SqlDbType.VarChar;
            switch (type)
            {
                case TkDataType.String:
                    dbType = SqlDbType.VarChar;
                    break;

                case TkDataType.Int:
                    dbType = SqlDbType.Int;
                    break; ;
                case TkDataType.Date:
                case TkDataType.DateTime:
                    dbType = SqlDbType.DateTime;
                    break;

                case TkDataType.Double:
                    dbType = SqlDbType.Float;
                    break;

                case TkDataType.Text:
                    dbType = SqlDbType.Text;
                    break;

                case TkDataType.Money:
                    dbType = SqlDbType.Money;
                    break;

                case TkDataType.Blob:
                    dbType = SqlDbType.Image;
                    break;

                case TkDataType.Binary:
                    dbType = SqlDbType.Binary;
                    break;

                case TkDataType.Guid:
                    dbType = SqlDbType.UniqueIdentifier;
                    break;

                case TkDataType.Xml:
                    dbType = SqlDbType.Xml;
                    break;

                case TkDataType.Bit:
                    dbType = SqlDbType.Bit;
                    break;

                case TkDataType.Byte:
                    dbType = SqlDbType.TinyInt;
                    break;

                case TkDataType.Short:
                    dbType = SqlDbType.SmallInt;
                    break;

                case TkDataType.Long:
                    dbType = SqlDbType.BigInt;
                    break;

                case TkDataType.Decimal:
                    dbType = SqlDbType.Decimal;
                    break;
                    //case TkDataType.Geography:
                    //    dbType = SqlDbType.Udt;
                    //    param.UdtTypeName = "geography";//"Hello.sys.geography";
                    //    break;
            }
            param.SqlDbType = dbType;

            return param;
        }

        string IDbProvider.GetSqlParamName(string fieldName, bool isOrigin)
        {
            TkDebug.AssertArgumentNullOrEmpty(fieldName, "fieldName", this);

            return GetParamName(fieldName, isOrigin);
        }

        string IDbProvider.GetParamName(string fieldName, bool isOrigin)
        {
            TkDebug.AssertArgumentNullOrEmpty(fieldName, "fieldName", this);

            return GetParamName(fieldName, isOrigin);
        }

        #endregion IDbProvider 成员

        private static string GetParamName(string fieldName, bool isOrigin)
        {
            return isOrigin ? "@OLD_" + fieldName : "@" + fieldName;
        }

        public override string ToString()
        {
            return DESCRIPTION;
        }
    }
}