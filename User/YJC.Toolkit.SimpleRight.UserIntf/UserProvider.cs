using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    internal class UserProvider : IUserProvider
    {
        private readonly DbContextConfig fDbConfig;
        private IPasswordProvider fPasswordProvider;

        internal UserProvider(UserManager manager, DbContextConfig dbConfig)
        {
            Manager = manager;
            fDbConfig = dbConfig;
        }

        #region IUserProvider 成员

        public bool ChangePassword(string logOnName, string oldPassword, string newPassword)
        {
            oldPassword = PasswordProvider.Format(oldPassword);
            newPassword = PasswordProvider.Format(newPassword);

            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver fResolver = GetUserTableResolver(source))
            {
                DataRow row = fResolver.TrySelectRowWithParam("LoginName", logOnName);
                if (row == null)
                    return false;

                if (!row["LoginPasswd"].ToString().Equals(oldPassword))
                    return false;
                else
                {
                    row.BeginEdit();
                    try
                    {
                        row["LoginPasswd"] = newPassword;
                        row["PasswdChangeDate"] = DateTime.Now;
                    }
                    finally
                    {
                        row.EndEdit();
                    }
                    fResolver.SetCommands(AdapterCommand.Update);
                    fResolver.UpdateDatabase();
                    return true;
                }
            }
        }

        public bool ChangePasswordQuestionAndAnswer(string logOnName, string password,
            string newPasswordQuestion, string newPasswordAnswer)
        {
            return ChangePassword(logOnName, password, newPasswordAnswer);
        }

        public void CreateUser(IUser user)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver fResolver = GetUserTableResolver(source))
            {
                DataRow fRow = fResolver.NewRow();
                fRow.BeginEdit();
                try
                {
                    fRow["Id"] = user.Id;
                    fRow["Name"] = user.Name;
                    fRow["LoginName"] = user.LogOnName;
                    fRow["LoginPasswd"] = user.GetPassword();
                    fRow["LoginDate"] = user.LastLogOnDate;
                    fRow["Phone"] = user.Phone;
                    fRow["Email"] = user.Email;
                    fRow["WorkNo"] = user.Number;
                    fRow["CreateDate"] = user.CreationDate;
                    fRow["PasswdChangeDate"] = user.LastPasswordChangedDate;
                    fRow["CreateId"] = -1;

                    fRow["Active"] = user.IsApproved;
                }
                finally
                {
                    fRow.EndEdit();
                }
                fResolver.SetCommands(AdapterCommand.Insert);
                fResolver.UpdateDatabase();
            }
        }

        public bool DeleteUser(string logOnName, bool deleteAllRelatedData)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver fResolver = GetUserTableResolver(source))
            {
                DataRow row = fResolver.TrySelectRowWithParam("LoginName", logOnName);
                if (row == null)
                {
                    return false;
                }
                else
                {
                    fResolver.Delete();
                    fResolver.UpdateDatabase();
                    return true;
                }
            }
        }

        public IEnumerable<IUser> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            return GetUsersMatchField("USER_EMAIL", emailToMatch, pageIndex, pageSize, out totalRecords);
        }

        public IEnumerable<IUser> FindUsersByPhone(string phoneToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            return GetUsersMatchField("USER_PHONE", phoneToMatch, pageIndex, pageSize, out totalRecords);
        }

        public IEnumerable<IUser> FindUsersByNumber(string numberToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            return GetUsersMatchField("USER_WORK_NO", numberToMatch, pageIndex, pageSize, out totalRecords);
        }

        public IEnumerable<IUser> FindUsersByName(string fullnameToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            return GetUsersMatchField("USER_NAME", fullnameToMatch, pageIndex, pageSize, out totalRecords);
        }

        public IEnumerable<IUser> GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            return GetUsersMatchField(null, null, pageIndex, pageSize, out totalRecords);
        }

        public int NumberOfUsersOnline
        {
            get
            {
                return 1;
            }
        }

        public string GetPassword(string logOnName, string answer)
        {
            TkDbContext context = fDbConfig.CreateDbContext();
            using (context)
            {
                IParamBuilder builder = SqlParamBuilder.CreateEqualSql(context,
                    "USER_LOGIN_NAME", TkDataType.String, logOnName);
                return DbUtil.ExecuteScalar("SELECT USER_LOGIN_PASSWD FROM UR_USERS", context, builder).ToString();
            }
        }

        public IUser GetUser(string logOnName, bool userIsOnline)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver fResolver = GetUserTableResolver(source))
            {
                userIsOnline = true;
                DataRow row = fResolver.TrySelectRowWithParam("LoginName", logOnName);
                if (row == null)
                    return null;
                else
                {
                    return new User(row, this);
                }
            }
        }

        public string GetUserNameByEmail(string email)
        {
            DataRow row = GetRowWithParam("Email", email);
            return row["Email"].ToString();
        }

        public string ResetPassword(string logOnName, string answer)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver fResolver = GetUserTableResolver(source))
            {
                DataRow row = fResolver.TrySelectRowWithParam("LoginName", logOnName);
                if (row == null)
                {
                    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                        "需要设置密码的登录名为{0}的用户不存在", logOnName), this);
                    return "";
                }
                else
                {
                    string newPasswd = System.Guid.NewGuid().ToString().Substring(0, 10);
                    row.BeginEdit();
                    try
                    {
                        row["LoginPasswd"] = PasswordProvider.Format(newPasswd);
                        row["PasswdChangeDate"] = DateTime.Now;

                        row["UpdateId"] = -1;
                        row["UpdateDate"] = DateTime.Now;
                    }
                    finally
                    {
                        row.EndEdit();
                    }
                    fResolver.SetCommands(AdapterCommand.Update);
                    fResolver.UpdateDatabase();
                    return newPasswd;
                }
            }
        }

        public bool UnlockUser(string logOnName)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver fResolver = GetUserTableResolver(source))
            {
                DataRow row = fResolver.TrySelectRowWithParam("USER_LOGIN_NAME", logOnName);
                if (row == null)
                {
                    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                        "需要解锁的登录名为{0}的用户不存在", logOnName), this);
                    return false;
                }
                else
                {
                    row.BeginEdit();
                    try
                    {
                        if (row["Active"].Value<int>() == 0)
                        {
                            row["Active"] = 1;
                        }

                        DateTime now = DateTime.Now;
                        row["UnlockDate"] = now;
                        row["UpdateId"] = -1;
                        row["UpdateDate"] = now;
                    }
                    finally
                    {
                        row.EndEdit();
                    }
                    fResolver.SetCommands(AdapterCommand.Update);
                    fResolver.UpdateDatabase();
                    return true;
                }
            }
        }

        public void UpdateUser(IUser user)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver fResolver = GetUserTableResolver(source))
            {
                DateTime now = DateTime.Now;
                DataRow row = fResolver.SelectRowWithKeys(user.Id);
                row.BeginEdit();
                try
                {
                    row["Id"] = user.Id;
                    row["Name"] = user.Name;
                    row["LoginName"] = user.LogOnName;
                    row["LoginPasswd"] = user.GetPassword();
                    row["LoginDate"] = user.LastLogOnDate;
                    row["Phone"] = user.Phone;
                    row["Email"] = user.Email;
                    row["WorkNo"] = user.Number;
                    //row["CREATE_DATE"] = user.CreationDate;
                    row["PasswdChangeDate"] = user.LastPasswordChangedDate;
                    row["UpdateId"] = -1;
                    row["UpdateDate"] = now;
                }
                finally
                {
                    row.EndEdit();
                }
                fResolver.SetCommands(AdapterCommand.Update);
                fResolver.UpdateDatabase();
            }
        }

        public bool ValidateUser(string logOnName, string password)
        {
            password = PasswordProvider.Format(password);
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver fResolver = GetUserTableResolver(source))
            {
                DataRow row = fResolver.TrySelectRowWithParams(new string[] { "LoginName", "LoginPasswd" },
                    logOnName, password);
                return row != null;
            }
        }

        public string ApplicationName
        {
            get
            {
                return string.Empty;
            }
        }

        public bool EnablePasswordReset
        {
            get
            {
                return true;
            }
        }

        public bool EnablePasswordRetrieval
        {
            get
            {
                return true;
            }
        }

        public int MaxInvalidPasswordAttempts
        {
            get
            {
                return 3;
            }
        }

        public int PasswordAttemptWindow
        {
            get
            {
                return 10;
            }
        }

        public bool RequiresQuestionAndAnswer
        {
            get
            {
                return false;
            }
        }

        public bool RequiresUniqueEmail
        {
            get
            {
                return true;
            }
        }

        public IPasswordProvider PasswordProvider
        {
            get
            {
                if (fPasswordProvider == null)
                {
                    fPasswordProvider = new PasswordProvider();
                }
                return fPasswordProvider;
            }
        }

        public IUser GetUser(string userId)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver fResolver = GetUserTableResolver(source))
            {
                DataRow row = fResolver.TrySelectRowWithKeys(userId);
                if (row == null)
                    return null;
                else
                {
                    return new User(row, this);
                }
            }
        }

        /// <summary>
        /// 后代组织的用户  和 自身 不包括当前组织的其他用户
        /// </summary>
        /// <param name="logOnName"></param>
        /// <returns></returns>
        public IEnumerable<IUser> GetAllUsersInDescendantOrganization(string logOnName)
        {
            //throw new NotImplementedException();
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetUserTableResolver(source))
            {
                string sql = " USER_ORG_ID IN (SELECT ORG_ID FROM SYS_ORGANIZATION WHERE ORG_LAYER LIKE" +
                                 "(select ORG_LAYER FROM SYS_ORGANIZATION WHERE ORG_ID IN (" +
                                     "SELECT USER_ORG_ID FROM UR_USERS WHERE USER_LOGIN_NAME = {0} ))" +
                                   "+'%'  " +
                                  "AND ORG_ID <> (SELECT USER_ORG_ID FROM UR_USERS WHERE USER_LOGIN_NAME = {0} ) " +
                               "OR USER_LOGIN_NAME = {0} )";

                sql = string.Format(ObjectUtil.SysCulture, sql, source.Context.GetSqlParamName("USER_LOGIN_NAME"));
                IParamBuilder builder = ParamBuilder.CreateParamBuilder(sql,
                    "USER_LOGIN_NAME", TkDataType.String, logOnName);
                resolver.Select(builder);
                foreach (DataRow nodeRow in source.DataSet.Tables[resolver.TableName].Rows)
                {
                    yield return new User(nodeRow, this);
                }
            }
            //return null;
        }

        #endregion IUserProvider 成员

        public UserManager Manager { get; private set; }

        #region 私有方法

        internal static TableResolver GetUserTableResolver(IDbDataSource source)
        {
            TableResolver resolver = new TableResolver(MetaDataUtil.CreateTableScheme("Users.xml"), source)
            {
                FakeDelete = new FakeDeleteInfo("Active", "0")
            };
            return resolver;
        }

        private DataRow GetRowWithParam(string field, object value)
        {
            DataRow row = null;
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver fResolver = GetUserTableResolver(source))
            {
                row = fResolver.TrySelectRowWithParam(field, value);
            }
            return row;
        }

        private IEnumerable<IUser> GetUsersMatchField(string mathField, string matchValue, int pageIndex, int pageSize,
            out int totalRecords)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver fResolver = GetUserTableResolver(source))
            {
                TkDbContext context = source.Context;
                IParamBuilder builder = null;
                if (!string.IsNullOrEmpty(mathField) && !string.IsNullOrEmpty(matchValue))
                {
                    builder = SqlParamBuilder.CreateSingleSql(context, mathField, TkDataType.String,
                       "LIKE", string.Format(ObjectUtil.SysCulture, "%{0}%", matchValue));
                }
                else
                    builder = SqlParamBuilder.CreateSql(" 1 = 1");

                totalRecords = DbUtil.ExecuteScalar("SELECT COUNT(*) FROM UR_USERS ", context, builder).Value<int>();
                int number = (pageIndex + 1) * pageSize;
                string whereSql = "where " + builder.Sql;
                using (SqlSelector sqlSelect = new SqlSelector(context, source.DataSet))
                {
                    ISimpleAdapter adapter = sqlSelect;
                    IListSqlContext sqlContext = fDbConfig.GetListSql(fResolver.Fields, "UR_USERS", fResolver.GetKeyFieldArray(),
                        whereSql, "ORDER BY USER_ID", number - pageSize, number);

                    adapter.SetSql(sqlContext.ListSql, builder);
                    fDbConfig.SetListData(sqlContext, adapter, source.DataSet, number - pageSize, pageSize, "UR_USERS");
                    return GetArrayFromTable(source.DataSet.Tables[fResolver.TableName]);
                }
            }
        }

        #endregion 私有方法

        private IEnumerable<IUser> GetArrayFromTable(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                yield return new User(row, this);
            }
        }
    }
}