using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.MetaData
{
    public interface INormalTableData : ITableData
    {
        int ColumnCount { get; }

        bool HasEditKey { get; }

        bool IsFix { get; }

        TableShowStyle ListStyle { get; }

        IPageStyle Style { get; }

        ITableOutput Output { get; }
    }
}