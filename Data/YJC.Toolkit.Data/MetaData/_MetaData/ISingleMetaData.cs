using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public interface ISingleMetaData
    {
        int ColumnCount { get; }

        bool CommitDetail { get; }

        JsonObjectType JsonDataType { get; }

        Tk5TableScheme CreateTableScheme(ITableSchemeEx scheme, IInputData input);

        ITableSchemeEx CreateSourceScheme(IInputData input);
    }
}
