using System.Collections.Generic;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;

namespace YJC.Toolkit.Data
{
    public interface IListModel : IModel
    {
        string PageStyle { get; }

        string Source { get; }

        bool HasListButtons { get; }

        IEnumerable<Operator> ListOperators { get; }

        CountInfo PageInfo { get; }

        ListSortInfo SortInfo { get; }

        string DecoderSelfUrl { get; }

        IEnumerable<Operator> RowOperators { get; }

        IEnumerable<ListTabSheet> TabSheets { get; }

        IEnumerable<IOperatorFieldProvider> CreateDataRowsProvider(IListMetaData metaData);

        IFieldValueProvider CreateEmptyProvider();

        IFieldValueProvider CreateQueryProvider();

        IFieldValueProvider CreateProvider(string tableName);
    }
}