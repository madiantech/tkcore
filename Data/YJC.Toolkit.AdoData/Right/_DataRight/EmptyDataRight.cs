using YJC.Toolkit.Data;

namespace YJC.Toolkit.Right
{
    public class EmptyDataRight : IDataRight, IRightCustomMessage
    {
        public EmptyDataRight(bool allowAll)
        {
            AllowAll = allowAll;
        }

        #region IDataRight 成员

        public IParamBuilder GetListSql(ListDataRightEventArgs e)
        {
            return AllowAll ? null : SqlParamBuilder.NoResult;
        }

        public void Check(DataRightEventArgs e)
        {
            if (!AllowAll)
                throw new NoDataRightException(ErrorMessage);
        }

        #endregion

        public bool AllowAll { get; private set; }

        public string ErrorMessage { get; set; }
    }
}
