using YJC.Toolkit.Data;

namespace YJC.Toolkit.Right
{
    public interface IDataRight
    {
        IParamBuilder GetListSql(ListDataRightEventArgs e);

        void Check(DataRightEventArgs e);
    }
}
