using System.Collections.Generic;

namespace YJC.Toolkit.MetaData
{
    public interface ITableScheme : IFieldInfoIndexer
    {
        string TableName { get; }

        IEnumerable<IFieldInfo> Fields { get; }

        IEnumerable<IFieldInfo> AllFields { get; }
    }
}