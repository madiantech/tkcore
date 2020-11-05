using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    internal class RoleProvider : IRoleProvider
    {
        private readonly DbContextConfig fDbConfig;
        private List<IRole> fAllRoles;

        public RoleProvider(UserManager manager, DbContextConfig dbConfig)
        {
            Manager = manager;
            fDbConfig = dbConfig;
        }

        #region IRoleProvider 成员

        public void AddUserToRole(IRole role, params IUser[] users)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetUserPartTableResolver(source))
            {
                foreach (IUser user in users)
                {
                    DataRow row = resolver.NewRow();
                    row.BeginEdit();
                    try
                    {
                        row["UserId"] = user.Id;
                        row["PartId"] = role.Id;
                    }
                    finally
                    {
                        row.EndEdit();
                    }
                }
                resolver.SetCommands(AdapterCommand.Insert);
                resolver.UpdateDatabase();
            }
        }

        public void AddUsersToRoles(IDictionary<IRole, IEnumerable<IUser>> list)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetUserPartTableResolver(source))
            {
                foreach (KeyValuePair<IRole, IEnumerable<IUser>> userList in list)
                {
                    foreach (IUser user in userList.Value)
                    {
                        DataRow row = resolver.NewRow();
                        row.BeginEdit();
                        try
                        {
                            row["UserId"] = user.Id;
                            row["PartId"] = userList.Key.Id;
                        }
                        finally
                        {
                            row.EndEdit();
                        }
                    }
                }
                if (list.Count > 0)
                {
                    resolver.SetCommands(AdapterCommand.Insert);
                    resolver.UpdateDatabase();
                }
            }
        }

        public IEnumerable<IRole> AllRoles
        {
            get
            {
                if (fAllRoles == null)
                {
                    fAllRoles = new List<IRole>();
                    SetAllRoles();
                }
                return fAllRoles;
            }
        }

        public string ApplicationName
        {
            get
            {
                return string.Empty;
            }
        }

        public void CreateRole(IRole role)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetPartTableResolver(source))
            {
                DataRow row = resolver.NewRow();
                row.BeginEdit();
                try
                {
                    row["Id"] = role.Id;
                    row["Name"] = role.Name;
                }
                finally
                {
                    row.EndEdit();
                }
                resolver.SetCommands(AdapterCommand.Insert);
                resolver.UpdateDatabase();
            }
        }

        public bool DeleteRole(string roleCode, bool throwOnPopulatedRole)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetPartTableResolver(source))
            {
                if (throwOnPopulatedRole)
                {
                    string sql = "SELECT COUNT(*) FROM UR_USERS WHERE USER_ID IN "
                        + "(SELECT UP_USER_ID FROM UR_USERS_PART WHERE UP_PART_ID = @UP_PART_ID )";
                    DbParameterList list = new DbParameterList();
                    list.Add("UP_PART_ID", TkDataType.String, roleCode);
                    int count = DbUtil.ExecuteScalar(sql, source.Context, list).Value<int>();
                    if (count > 0)
                    {
                        TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                            "编号为{0}的权限被{1}个用户使用，无法删除", roleCode, count), this);
                    }
                }
                DataRow row = resolver.TrySelectRowWithKeys(roleCode);
                if (row != null)
                {
                    resolver.Delete();
                    resolver.UpdateDatabase();
                    return true;
                }
                else
                    return false;
            }
        }

        public IRole GetRole(string roleCode)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetPartTableResolver(source))
            {
                DataRow row = resolver.TrySelectRowWithKeys(roleCode);
                if (row != null)
                {
                    return new Role(row, this);
                }
                else
                    return null;
            }
        }

        public IEnumerable<IRole> GetRolesForUser(string logOnName)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetPartTableResolver(source))
            {
                string tableName = resolver.TableName;
                string sql = string.Format(ObjectUtil.SysCulture, " PART_ID IN (SELECT DISTINCT "
                   + " UP_PART_ID FROM UR_USERS_PART WHERE UP_USER_ID IN (SELECT USER_ID FROM "
                   + " UR_USERS WHERE USER_LOGIN_NAME = {0}) )", source.Context.GetSqlParamName("USER_LOGIN_NAME"));
                IParamBuilder builder = ParamBuilder.CreateParamBuilder(sql, "USER_LOGIN_NAME",
                    TkDataType.String, logOnName);

                resolver.Select(builder);
                foreach (DataRow row in source.DataSet.Tables[tableName].Rows)
                {
                    yield return new Role(row, this);
                }
            }
        }

        public IEnumerable<IUser> GetUsersInRole(string roleCode)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = UserProvider.GetUserTableResolver(source))
            {
                string tableName = resolver.TableName;
                string sql = string.Format(ObjectUtil.SysCulture, @" USER_ID IN "
                    + " (SELECT UP_USER_ID FROM UR_USERS_PART WHERE UP_PART_ID = {0})",
                    source.Context.GetSqlParamName("UP_PART_ID"));
                IParamBuilder builder = ParamBuilder.CreateParamBuilder(sql,
                    "UP_PART_ID", TkDataType.String, roleCode);

                resolver.Select(builder);
                foreach (DataRow row in source.DataSet.Tables[resolver.TableName].Rows)
                {
                    yield return new User(row, Manager.UserProvider.Convert<UserProvider>());
                }
            }
        }

        public bool IsUserInRole(string logOnName, string roleCode)
        {
            using (DataSet ds = new DataSet() { Locale = ObjectUtil.SysCulture })
            using (TkDbContext context = fDbConfig.CreateDbContext())
            {
                string sql = string.Format(ObjectUtil.SysCulture, @"SELECT COUNT(USER_ID) FROM "
                    + " UR_USERS WHERE USER_LOGIN_NAME = {0} AND (USER_ACTIVE = 1) AND USER_ID IN (SELECT UP_USER_ID FROM "
                    + " UR_USERS_PART WHERE UP_PART_ID = {1})", context.GetSqlParamName("USER_LOGIN_NAME"),
                    context.GetSqlParamName("UP_PART_ID"));
                DbParameterList list = new DbParameterList();
                list.Add("USER_LOGIN_NAME", TkDataType.String, logOnName);
                list.Add("UP_PART_ID", TkDataType.String, roleCode);

                return DbUtil.ExecuteScalar(sql, context, list).Value<int>() > 0;
            }
        }

        public void RemoveUserToRole(IRole role, params IUser[] users)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetUserPartTableResolver(source))
            {
                int count = users.Length;
                if (count > 0)
                {
                    string[] ids = new string[count];
                    for (int i = 0; i < count; i++)
                    {
                        ids[i] = users[i].Id;
                    }
                    resolver.SelectWithKeys(ids);
                    if (resolver.HostTable.Rows.Count > 0)
                    {
                        resolver.Delete();
                        resolver.UpdateDatabase();
                    }
                }
            }
        }

        public void RemoveUsersToRoles(IDictionary<IRole, IEnumerable<IUser>> list)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetUserPartTableResolver(source))
            {
                foreach (KeyValuePair<IRole, IEnumerable<IUser>> userList in list)
                {
                    foreach (IUser user in userList.Value)
                    {
                        resolver.SelectWithKeys(userList.Key, user.Id);
                    }
                }
                if (resolver.HostTable.Rows.Count > 0)
                {
                    resolver.Delete();
                }
                resolver.UpdateDatabase();
            }
        }

        public bool RoleExists(string roleCode)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetPartTableResolver(source))
            {
                return resolver.TrySelectRowWithKeys(roleCode) != null;
            }
        }

        #endregion IRoleProvider 成员

        public UserManager Manager { get; private set; }

        internal static TableResolver GetPartTableResolver(IDbDataSource source)
        {
            return new TableResolver(MetaDataUtil.CreateTableScheme("Part.xml"), source);
        }

        internal static TableResolver GetUserPartTableResolver(IDbDataSource source)
        {
            return new TableResolver(MetaDataUtil.CreateTableScheme("UsersPart.xml"), source);
        }

        private void SetAllRoles()
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver fResolver = GetPartTableResolver(source))
            {
                fResolver.Select();
                foreach (DataRow row in fResolver.HostTable.Rows)
                {
                    IRole role = new Role(row, this);
                    fAllRoles.Add(role);
                }
            }
        }
    }
}