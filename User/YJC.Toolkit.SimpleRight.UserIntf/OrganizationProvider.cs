using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    internal class OrganizationProvider : IOrganizationProvider
    {
        private bool fInitRoot;
        private IEnumerable<ITreeNode> fRootNodes;
        private int fTopLevel;
        private readonly DbContextConfig fDbConfig;

        public OrganizationProvider(UserManager manager, DbContextConfig dbConfig)
        {
            Manager = manager;
            fDbConfig = dbConfig;
        }

        #region IOrganizationProvider 成员

        public string ApplicationName
        {
            get
            {
                return string.Empty;
            }
        }

        public IUser FindUserInMainOrganization(string orgCode, string roleCode)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = UserProvider.GetUserTableResolver(source))
            {
                var context = source.Context;
                var ds = source.DataSet;

                string sql = string.Format(ObjectUtil.SysCulture, " USER_ORG_ID IN (SELECT ORG_ID "
                    + "FROM SYS_ORGANIZATION WHERE ORG_CODE = {0}) AND USER_ID IN (SELECT UP_USER_ID FROM "
                    + "UR_USERS_PART WHERE UP_PART_ID = {1})", context.GetSqlParamName("ORG_CODE"),
                    context.GetSqlParamName("UP_PART_ID"));

                DbParameterList list = new DbParameterList();
                list.Add("ORG_CODE", TkDataType.String, orgCode);
                list.Add("UP_PART_ID", TkDataType.String, roleCode);
                resolver.Select(ParamBuilder.CreateParamBuilder(sql, list));
                if (ds.Tables[resolver.TableName].Rows.Count > 0)
                {
                    return new User(ds.Tables[resolver.TableName].Rows[0],
                        Manager.UserProvider.Convert<UserProvider>());
                }
                else
                    return null;
            }
        }

        public IEnumerable<IUser> FindUsersByRole(string orgCode, string roleCode)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = UserProvider.GetUserTableResolver(source))
            {
                var context = source.Context;
                var ds = source.DataSet;

                string sql = string.Format(" USER_ORG_ID IN (SELECT ORG_ID FROM SYS_ORGANIZATION "
                     + " WHERE ORG_CODE = {0}) AND USER_ID IN (SELECT UP_USER_ID FROM UR_USERS_PART WHERE "
                     + " UP_PART_ID = {1} )", context.GetSqlParamName("ORG_CODE"), context.GetSqlParamName("UP_PART_ID"));
                DbParameterList list = new DbParameterList();
                list.Add("ORG_CODE", TkDataType.String, orgCode);
                list.Add("UP_PART_ID", TkDataType.String, roleCode);
                resolver.Select(ParamBuilder.CreateParamBuilder(sql, list));
                UserProvider provider = Manager.UserProvider.Convert<UserProvider>();
                foreach (DataRow row in ds.Tables[resolver.TableName].Rows)
                {
                    yield return new User(row, provider);
                }
            }
        }

        public IOrganization GetMainOrganizationsForUser(string logOnName)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetOrganizationTableResolver(source))
            {
                var context = source.Context;
                var ds = source.DataSet;

                string sql = string.Format(ObjectUtil.SysCulture, " ORG_ID IN (SELECT USER_ORG_ID FROM "
                   + " UR_USERS WHERE USER_LOGIN_NAME = {0})", context.GetSqlParamName("USER_LOGIN_NAME"));
                IParamBuilder builder = ParamBuilder.CreateParamBuilder(sql,
                    "USER_LOGIN_NAME", TkDataType.String, logOnName);

                resolver.Select(builder);
                if (ds.Tables[resolver.TableName].Rows.Count > 0)
                {
                    Organization org = new Organization(ds.Tables[resolver.TableName].Rows[0], this);
                    if (!org.IsDepartment)
                        return org;
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        public IOrganization GetOrganization(string orgCode)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (SqlSelector selector = new SqlSelector(source))
            using (TableResolver resolver = GetOrganizationTableResolver(source))
            {
                DataRow row = resolver.TrySelectRowWithParam("Code", orgCode);
                if (row != null)
                {
                    Organization org = new Organization(row, this);
                    if (!org.IsDepartment)
                        return org;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<IOrganization> GetOrganizationsForUser(string logOnName)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetOrganizationTableResolver(source))
            {
                string sql = string.Format(" ORG_ID IN (SELECT USER_ORG_ID FROM UR_USERS "
                    + " WHERE USER_LOGIN_NAME = {0})", source.Context.GetSqlParamName("USER_LOGIN_NAME"));
                IParamBuilder builder = ParamBuilder.CreateParamBuilder(sql,
                    "USER_LOGIN_NAME", TkDataType.String, logOnName);

                resolver.Select(builder);
                foreach (DataRow row in source.DataSet.Tables[resolver.TableName].Rows)
                {
                    Organization org = new Organization(row, this);
                    if (!org.IsDepartment)
                        yield return org;
                }
            }
        }

        public IOrganization GetParentOrganization(string orgCode)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetOrganizationTableResolver(source))
            {
                var context = source.Context;
                var ds = source.DataSet;

                string sql = string.Format(ObjectUtil.SysCulture, " ORG_ID IN (SELECT ORG_PARENT_ID "
                    + " FROM {0} WHERE ORG_CODE = {1})", resolver.TableName, context.GetSqlParamName("ORG_CODE"));
                IParamBuilder builder = ParamBuilder.CreateParamBuilder(sql,
                    "ORG_CODE", TkDataType.String, orgCode);

                resolver.Select(builder);
                if (ds.Tables[resolver.TableName].Rows.Count > 0)
                {
                    Organization org = new Organization(ds.Tables[resolver.TableName].Rows[0], this);
                    return org;
                }
                else
                    return null;
            }
        }

        public IUser GetUserInMainOrganization(string orgCode)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = UserProvider.GetUserTableResolver(source))
            {
                var context = source.Context;
                var ds = source.DataSet;

                string sql = string.Format(ObjectUtil.SysCulture, " USER_ORG_ID IN (SELECT "
                    + " ORG_ID FROM SYS_ORGANIZATION WHERE ORG_CODE = {0})", context.GetSqlParamName("ORG_CODE"));
                IParamBuilder builder = ParamBuilder.CreateParamBuilder(sql,
                   "ORG_CODE", TkDataType.String, orgCode);

                resolver.Select(builder);
                if (ds.Tables[resolver.TableName].Rows.Count > 0)
                {
                    return new User(ds.Tables[resolver.TableName].Rows[0],
                        Manager.UserProvider.Convert<UserProvider>());
                }
                else
                    return null;
            }
        }

        public IEnumerable<IUser> GetUsersInOrganization(string orgCode)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = UserProvider.GetUserTableResolver(source))
            {
                string sql = string.Format(" USER_ORG_ID IN (SELECT ORG_ID FROM SYS_ORGANIZATION "
                    + " WHERE ORG_CODE = {0})", source.Context.GetSqlParamName("ORG_CODE"));
                IParamBuilder builder = ParamBuilder.CreateParamBuilder(sql,
                   "ORG_CODE", TkDataType.String, orgCode);
                resolver.Select(builder);
                UserProvider provider = Manager.UserProvider.Convert<UserProvider>();
                foreach (DataRow row in source.DataSet.Tables[resolver.TableName].Rows)
                {
                    yield return new User(row, provider);
                }
            }
        }

        public bool IsUserInOrganization(string logOnName, string orgCode)
        {
            using (TkDbContext context = fDbConfig.CreateDbContext())
            {
                string sql = string.Format(ObjectUtil.SysCulture, " SELECT COUNT(USER_ID) FROM UR_USERS "
                    + " WHERE USER_LOGIN_NAME = {0} AND (USER_ACTIVE = 1) AND USER_ORG_ID IN (SELECT ORG_ID FROM SYS_ORGANIZATION "
                    + " WHERE ORG_CODE = {1})", context.GetSqlParamName("USER_LOGIN_NAME"), context.GetSqlParamName("ORG_CODE"));
                DbParameterList list = new DbParameterList();
                list.Add("USER_LOGIN_NAME", TkDataType.String, logOnName);
                list.Add("ORG_CODE", TkDataType.String, orgCode);
                return DbUtil.ExecuteScalar(sql, context, list).Value<int>() > 0;
            }
        }

        public IOrganization GetGrandParentDeparentment(string orgCode)
        {
            return GetGrandParent(orgCode, true);
        }

        public IOrganization GetGrandParentOrganization(string orgCode)
        {
            return GetGrandParent(orgCode, false);
        }

        public IOrganization GetParentDeparentment(string orgCode)
        {
            return GetParent(orgCode, true);
        }

        #endregion IOrganizationProvider 成员

        #region ITree 成员

        public IEnumerable<ITreeNode> GetChildNodes(string parentId)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetOrganizationTableResolver(source))
            {
                resolver.TrySelectRowWithParam("ParentId", parentId);
                foreach (DataRow row in source.DataSet.Tables[resolver.TableName].Rows)
                {
                    yield return new Organization(row, this) as ITreeNode;
                }
            }
        }

        public IEnumerable<ITreeNode> GetDisplayTreeNodes(string id)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetOrganizationTableResolver(source))
            {
                DataRow row = resolver.TrySelectRowWithKeys(id);
                if (row == null)
                    return Enumerable.Empty<IOrganization>();
                else
                {
                    string layer = row["Layer"].ToString();
                    int num = layer.Length / 3;
                    if (num < TopLevel)
                    {
                        return Enumerable.Empty<IOrganization>();
                    }
                    else
                    {
                        return GetDisplayTreeNodesInDataSet(layer, num, source.Context, resolver, source.DataSet).ToList();
                    }
                }
            }
        }

        public ITreeNode GetTreeNode(string id)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetOrganizationTableResolver(source))
            {
                DataRow row = resolver.TrySelectRowWithKeys(id);
                if (row != null)
                {
                    Organization org = new Organization(row, this);
                    return org as ITreeNode;
                }
                else
                    return null;
            }
        }

        public IEnumerable<ITreeNode> RootNodes
        {
            get
            {
                InitRootNode();
                return fRootNodes;
            }
        }

        public string RootParentId
        {
            get
            {
                return "-1";
            }
        }

        public int TopLevel
        {
            get
            {
                InitRootNode();
                return fTopLevel;
            }
        }

        #endregion ITree 成员

        public UserManager Manager { get; private set; }

        internal IEnumerable<IOrganization> GetChildren(string orgId)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetOrganizationTableResolver(source))
            {
                IParamBuilder builder = SqlParamBuilder.CreateEqualSql(source.Context,
                    "ORG_PARENT_ID", TkDataType.String, orgId);
                resolver.Select(builder);
                foreach (DataRow nodeRow in source.DataSet.Tables[resolver.TableName].Rows)
                {
                    yield return new Organization(nodeRow, this);
                }
            }
        }

        internal IEnumerable<IOrganization> GetDescendant(string orgId)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetOrganizationTableResolver(source))
            {
                var context = source.Context;
                var ds = source.DataSet;

                DataRow row = resolver.SelectRowWithKeys(orgId);
                string layer = row["Layer"].ToString();
                ds.Tables[resolver.TableName].Clear();
                IParamBuilder builder = SqlParamBuilder.CreateSingleSql(context, "ORG_LAYER",
                    TkDataType.String, "LIKE", layer + "%");
                resolver.Select(builder);
                foreach (DataRow nodeRow in ds.Tables[resolver.TableName].Rows)
                {
                    yield return new Organization(nodeRow, this);
                }
            }
        }

        #region 私有方法

        internal static TableResolver GetOrganizationTableResolver(IDbDataSource source)
        {
            return new TableResolver(MetaDataUtil.CreateTableScheme("Organization.xml"), source);
        }

        private IOrganization GetGrandParent(string orgCode, bool isDepartMent)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetOrganizationTableResolver(source))
            {
                var ds = source.DataSet;

                string sql = string.Format(ObjectUtil.SysCulture, " ORG_ID IN (SELECT ORG_PARENT_ID "
                    + " FROM {0} WHERE ORG_ID = (SELECT ORG_PARENT_ID FROM {0} WHERE ORG_CODE = {1}))",
                    resolver.TableName, source.Context.GetSqlParamName("ORG_CODE"));
                IParamBuilder builder = ParamBuilder.CreateParamBuilder(sql,
                    "ORG_CODE", TkDataType.String, orgCode);

                resolver.Select(builder);
                if (ds.Tables[resolver.TableName].Rows.Count > 0)
                {
                    Organization departMent = new Organization(ds.Tables[resolver.TableName].Rows[0], this);

                    if (departMent.IsDepartment == isDepartMent)
                        return departMent;
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        internal IOrganization GetOrgById(string orgId)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetOrganizationTableResolver(source))
            {
                DataRow row = resolver.TrySelectRowWithKeys(orgId);
                if (row != null)
                {
                    return new Organization(row, this);
                }
                return null;
            }
        }

        private IOrganization GetParent(string orgCode, bool isDepartMent)
        {
            using (UserDataSource source = new UserDataSource(fDbConfig))
            using (TableResolver resolver = GetOrganizationTableResolver(source))
            {
                var context = source.Context;
                var ds = source.DataSet;

                string sql = string.Format(ObjectUtil.SysCulture, " ORG_ID IN (SELECT ORG_PARENT_ID "
                    + " FROM {0} WHERE ORG_CODE = {1})", resolver.TableName, context.GetSqlParamName("ORG_CODE"));
                IParamBuilder builder = ParamBuilder.CreateParamBuilder(sql,
                    "ORG_CODE", TkDataType.String, orgCode);
                resolver.Select(builder);

                if (ds.Tables[resolver.TableName].Rows.Count > 0)
                {
                    Organization org = new Organization(ds.Tables[resolver.TableName].Rows[0], this);
                    return org;
                }
                else
                    return null;
            }
        }

        private void InitRootNode()
        {
            if (!fInitRoot)
            {
                fInitRoot = true;
                IEnumerable nodes = GetChildNodes(RootParentId);
                List<ITreeNode> nodeList = new List<ITreeNode>();
                foreach (var node in nodes)
                {
                    nodeList.Add(node as ITreeNode);
                }
                fRootNodes = nodeList.ToArray();
                SetTopLevel(nodeList[0] as ITreeNode);
            }
        }

        private void SetTopLevel(ITreeNode node)
        {
            Organization organization = node as Organization;
            if (organization == null)
                fTopLevel = 0;
            fTopLevel = organization.Layer.Length / 3;
        }

        private IEnumerable<ITreeNode> GetDisplayTreeNodesInDataSet(string layer, int num,
            TkDbContext context, TableResolver resolver, DataSet ds)
        {
            IParamBuilder[] builders = new IParamBuilder[(num - TopLevel) + 1];
            for (int i = 0; i < builders.Length; i++)
            {
                int length = ((TopLevel + i) - 1) * 3;
                string fieldValue = layer.Substring(0, length).PadRight(length + 3, '_');
                string paramName = "ORG_LAYER" + i.ToString(ObjectUtil.SysCulture);
                builders[i] = SqlParamBuilder.CreateSingleSql(context, "ORG_LAYER", TkDataType.String,
                    "LIKE", paramName, fieldValue);
            }
            DataRowCollection rows = resolver.HostTable.Rows;
            int start = rows.Count;
            resolver.Select(SqlParamBuilder.CreateParamBuilderWithOr(builders));
            int end = rows.Count;
            for (int i = start; i < end; ++i)
            {
                DataRow nodeRow = rows[i];
                yield return new Organization(nodeRow, this);
            }
        }

        #endregion 私有方法
    }
}