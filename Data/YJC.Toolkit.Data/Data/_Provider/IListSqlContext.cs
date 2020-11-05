namespace YJC.Toolkit.Data
{
    public interface IListSqlContext
    {
        string ListSql { get; }

        void JoinSql(string format);
    }
}
