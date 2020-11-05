namespace YJC.Toolkit.MetaData
{
    public interface IMetaData
    {
        string Title { get; set; }

        object ToToolkitObject();

        ITableSchemeEx GetTableScheme(string tableName);
    }
}
