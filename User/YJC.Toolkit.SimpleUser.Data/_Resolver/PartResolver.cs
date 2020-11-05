using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleRight
{
    /// <summary>
    /// UR_PART 表的数据访问对象
    /// </summary>
    [Resolver(Author = "YJC", CreateDate = "2014-07-08",
        Description = "角色表(UR_PART)的数据访问对象")]
    internal class PartResolver : Tk5TableResolver
    {
        internal const string DATAXML = "UserManager/Part.xml";

        public PartResolver(IDbDataSource source)
            : base(DATAXML, source)
        {
        }

        protected override void OnUpdatingRow(UpdatingEventArgs e)
        {
            base.OnUpdatingRow(e);

            switch (e.Status)
            {
                //case UpdateKind.Delete:
                //    int roleid = GlobalVariable.Request.QueryString["ID"].Value<int>();
                //    string error = IsUseingRole(roleid);
                //    if (!string.IsNullOrEmpty(error))
                //    {
                //        throw new ErrorPageException("删除操作被禁止", "该角色不能删除，因为已经被如下用户分配：  " + error);
                //    }
                //    else
                //    {
                //        break;
                //    }
                case UpdateKind.Insert:
                    e.Row["Id"] = CreateUniId();
                    break;
                case UpdateKind.Update:
                    break;
                default:
                    break;
            }
        }

        //private string IsUseingRole(int roleid)
        //{
        //    DataSet ds = new DataSet() { Locale = ObjectUtil.SysCulture };
        //    SqlSelector sqlselect = new SqlSelector(Context, ds);
        //    using (ds)
        //    using (sqlselect)
        //    {
        //        //DataColumn dc = new DataColumn();
        //        //DbContext context = GlobalVariable.DefaultDbContextConfig.CreateDbContext();
        //        IFieldInfo fi = new FieldItem("UP_PART_ID", XmlDataType.Int);
        //        IParamBuilder pi = SqlParamBuilder.CreateEqualSql(Context, fi, roleid);

        //        sqlselect.Select("UR_USERS", "SELECT UR_USERS.USER_NAME , UR_USERS.USER_LOGIN_NAME FROM " +
        //            "  UR_USERS_PART LEFT JOIN UR_USERS ON  UR_USERS.USER_ID = UR_USERS_PART.UP_USER_ID ", pi);

        //        if (ds.Tables["UR_USERS"].Rows.Count > 0)
        //        {
        //            //  ds.Tables["UR_USERS"].AsEnumerable().Join(;
        //            // DataRow row = new DataRow();

        //            var strs = ds.Tables["UR_USERS"].AsEnumerable().Select(
        //                 item => string.Format(ObjectUtil.SysCulture, "{0}({1})", item["USER_NAME"].ToString(), item["USER_LOGIN_NAME"].ToString()));
        //            return strs.JoinString(",");
        //            // new { Str = row["USER_NAME"].ToString()+"("+row["USER_LOGIN_NAME"].ToString()+")" }
        //        }
        //        return null;
        //    }

        //    //int count = DataSetUtil.ExecuteScalar("SELECT COUNT(*) FROM UR_USERS_PART", Context, pi).Value<int>();
        //    //return count == 0;
        //}
    }
}
