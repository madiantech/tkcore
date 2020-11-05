using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    public class CompanyUserDataRight : IDataRight, IRightCustomMessage
    {
        private const string SQL = "SELECT ORG_LAYER FROM SYS_ORGANIZATION";
        private const string RETURN_SQL = "{0} = '{2}' OR ({0} IN (SELECT USER_ID FROM UR_USERS WHERE "
            + ORG_SQL + "))";
        private const string USER_SQL = "SELECT COUNT(*) FROM UR_USERS WHERE USER_ID = '{0}' AND " + ORG_SQL;
        private const string ORG_SQL = "USER_ORG_ID IN (SELECT ORG_ID FROM SYS_ORGANIZATION WHERE "
            + "ORG_LAYER LIKE '{1}%' AND ORG_LAYER != '{1}')";

        private readonly IFieldInfo fField;

        public CompanyUserDataRight(IFieldInfo field)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            fField = field;
        }

        #region IDataRight 成员

        public void Check(DataRightEventArgs e)
        {
            if (e.User.UserId == null || e.User.MainOrgId == null)
                throw new NoDataRightException(ErrorMessage);

            string userId = e.Row[fField.NickName].ToString();
            if (userId != e.User.UserId.ToString())
            {
                string layer = GetLayer(e);
                string sql = string.Format(ObjectUtil.SysCulture, USER_SQL, e.User.UserId, layer);
                int count = DbUtil.ExecuteScalar(sql, e.Context).Value<int>();
                if (count != 1)
                    throw new NoDataRightException(ErrorMessage);
            }
        }

        public IParamBuilder GetListSql(ListDataRightEventArgs e)
        {
            if (e.User.UserId == null || e.User.MainOrgId == null)
                return SqlParamBuilder.NoResult;

            string layer = GetLayer(e);
            string sql = string.Format(ObjectUtil.SysCulture, RETURN_SQL, fField.FieldName, layer, e.User.UserId);
            return ParamBuilder.CreateSql(sql);
        }

        #endregion

        public string ErrorMessage { get; set; }

        internal static string GetLayer(ListDataRightEventArgs e)
        {
            IParamBuilder builder = SqlParamBuilder.CreateEqualSql(e.Context, "ORG_ID", TkDataType.Int, e.User.MainOrgId);
            string layer = DbUtil.ExecuteScalar(SQL, e.Context, builder).ToString();
            return layer;
        }
    }
}
