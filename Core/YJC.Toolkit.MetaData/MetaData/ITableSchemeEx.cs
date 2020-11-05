using System.Collections.Generic;

namespace YJC.Toolkit.MetaData
{
    public interface ITableSchemeEx : IFieldInfoIndexer
    {
        string TableName { get; }

        string TableDesc { get; }

        IFieldInfoEx NameField { get; }

        IEnumerable<IFieldInfoEx> Fields { get; }

        IEnumerable<IFieldInfoEx> AllFields { get; }
    }
}