using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    public class CompanyOrgDataRight : IDataRight, IRightCustomMessage
    {
        private const string RETURN_SQL = "{0} IN " + ORG_SQL;
        private const string SINGLE_ORG_SQL = "SELECT COUNT(*) FROM SYS_ORGANIZATION WHERE ORG_ID = '{0}' AND ORG_ID IN " + ORG_SQL;
        private const string ORG_SQL = "(SELECT ORG_ID FROM SYS_ORGANIZATION WHERE ORG_LAYER LIKE '{1}%')";

        private readonly IFieldInfo fField;

        public CompanyOrgDataRight(IFieldInfo field)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            fField = field;
        }

        #region IRightCustomMessage 成员

        public string ErrorMessage { get; set; }

        #endregion

        #region IDataRight 成员

        public void Check(DataRightEventArgs e)
        {
            if (e.User.MainOrgId == null)
                throw new NoDataRightException(ErrorMessage);

            string orgId = e.Row[fField.NickName].ToString();
            if (orgId != e.User.MainOrgId.ToString())
            {
                string layer = CompanyUserDataRight.GetLayer(e);
                string sql = string.Format(ObjectUtil.SysCulture, SINGLE_ORG_SQL, e.User.MainOrgId, layer);
                int count = DbUtil.ExecuteScalar(sql, e.Context).Value<int>();
                if (count != 1)
                    throw new NoDataRightException(ErrorMessage);
            }
        }

        public IParamBuilder GetListSql(ListDataRightEventArgs e)
        {
            if (e.User.MainOrgId == null)
                return SqlParamBuilder.NoResult;

            string layer = CompanyUserDataRight.GetLayer(e);
            string sql = string.Format(ObjectUtil.SysCulture, RETURN_SQL, fField.FieldName, layer);
            return ParamBuilder.CreateSql(sql);
        }

        #endregion
    }
}
