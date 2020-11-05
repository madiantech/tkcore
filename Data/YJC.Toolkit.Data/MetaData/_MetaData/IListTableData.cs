using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.MetaData
{
    public interface IListTableData : ITableData
    {
        IEnumerable<Tk5FieldInfoEx> QueryFields { get; }

        ITableOutput Output { get; }
    }
}