using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class OwnerOrgDataRight : IDataRight, IRightCustomMessage
    {
        private readonly IFieldInfo fField;

        public OwnerOrgDataRight(IFieldInfo field)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            fField = field;
        }

        #region IDataRight 成员

        public IParamBuilder GetListSql(ListDataRightEventArgs e)
        {
            if (e.User.MainOrgId == null)
                return SqlParamBuilder.NoResult;

            return SqlParamBuilder.CreateEqualSql(e.Context, fField, e.User.MainOrgId);
        }

        public void Check(DataRightEventArgs e)
        {
            if (e.User.MainOrgId == null)
                throw new NoDataRightException(ErrorMessage);

            if (e.Row[fField.NickName].ToString() != e.User.MainOrgId.ToString())
                throw new NoDataRightException(ErrorMessage);
        }

        #endregion

        public string ErrorMessage { get; set; }
    }
}
