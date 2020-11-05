namespace YJC.Toolkit.Razor
{
    public interface ITableDataIndexer
    {
        SingleTableDetailData this[string tableName] { get; }
    }
}