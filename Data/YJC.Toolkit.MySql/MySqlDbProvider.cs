using System.Data;
using MySql.Data.MySqlClient;
using YJC.Toolkit.Cache;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;

namespace MySqlDbProvider
{
    [CacheInstance]
    [AlwaysCache]
    [DbProvider(RegName = REG_NAME, Author = "GLQ", CreateDate = "2016-12-20", Description = DESCRIPTION)]
    public class MySqlDbProvider : IDbProvider
    {
        internal const string REG_NAME = "MySql";
        internal const string DESCRIPTION = "MySql数据库连接提供者";

        #region IDbProvider 成员

        IDbConnection IDbProvider.CreateConnection()
        {
            return new MySqlConnection();
        }

        IDbDataAdapter IDbProvider.CreateDataAdapter()
        {
            return new MySqlDataAdapter();
        }

        IDbCommand IDbProvider.CreateCommand()
        {
            MySqlCommand command = new MySqlCommand();
            if (BaseAppSetting.Current != null)
                command.CommandTimeout = BaseAppSetting.Current.CommandTimeout;
            return command;
        }

        IDbDataParameter IDbProvider.CreateParameter(TkDataType type)
        {
            MySqlParameter param = new MySqlParameter();
            MySqlDbType dbType = MySqlDbType.VarChar;
            switch (type)
            {
                case TkDataType.String:
                    dbType = MySqlDbType.VarChar;
                    break;

                case TkDataType.Int:
                    dbType = MySqlDbType.Int32;
                    break; ;
                case TkDataType.Date:
                    dbType = MySqlDbType.Date;
                    break;

                case TkDataType.DateTime:
                    dbType = MySqlDbType.DateTime;
                    break;

                case TkDataType.Double:
                    dbType = MySqlDbType.Double;
                    break;

                case TkDataType.Text:
                    dbType = MySqlDbType.Text;
                    break;

                case TkDataType.Money:
                    dbType = MySqlDbType.Decimal;
                    break;

                case TkDataType.Blob:
                    dbType = MySqlDbType.Blob;
                    break;

                case TkDataType.Binary:
                    dbType = MySqlDbType.Binary;
                    break;

                case TkDataType.Guid:
                    dbType = MySqlDbType.Guid;
                    break;

                case TkDataType.Bit:
                    dbType = MySqlDbType.Bit;
                    break;

                case TkDataType.Byte:
                    dbType = MySqlDbType.Byte;
                    break;

                case TkDataType.Short:
                    dbType = MySqlDbType.Int16;
                    break;

                case TkDataType.Long:
                    dbType = MySqlDbType.Int64;
                    break;

                case TkDataType.Decimal:
                    dbType = MySqlDbType.Decimal;
                    break;

                case TkDataType.Xml:
                case TkDataType.Geography:
                    TkDebug.ThrowToolkitException(
                        string.Format(ObjectUtil.SysCulture,
                        "MySql不支持{0}类型", type.ToString()), this);
                    break;
            }
            param.MySqlDbType = dbType;

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