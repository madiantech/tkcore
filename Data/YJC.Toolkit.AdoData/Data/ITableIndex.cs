using System.Collections.Generic;

namespace YJC.Toolkit.Data
{
    public interface ITableIndex
    {
        string Name { get; }

        IEnumerable<IndexField> Fields { get; }
    }
}
