using System;
using System.Collections.Generic;

namespace YJC.Toolkit.Right
{
    public interface IColumnRight
    {
        IEnumerable<Tuple<string, ColumnDisplay>> GetColumnRestrict(ColumnDataRightEventArgs e);
    }
}
