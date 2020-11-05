using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public class OwnerDataRight : IDataRight, IRightCustomMessage
    {
        private readonly IFieldInfo fField;

        public OwnerDataRight(IFieldInfo field)
        {
            TkDebug.AssertArgumentNull(field, "field", null);

            fField = field;
        }

        #region IDataRight 成员

        public IParamBuilder GetListSql(ListDataRightEventArgs e)
        {
            if (e.User.UserId == null)
                return SqlParamBuilder.NoResult;

            return SqlParamBuilder.CreateEqualSql(e.Context, fField, e.User.UserId);
        }

        public void Check(DataRightEventArgs e)
        {
            if (e.User.UserId == null)
                throw new NoDataRightException(ErrorMessage);

            if (e.Row[fField.NickName].ToString() != e.User.UserId.ToString())
                throw new NoDataRightException(ErrorMessage);
        }

        #endregion

        public string ErrorMessage { get; set; }
    }
}
