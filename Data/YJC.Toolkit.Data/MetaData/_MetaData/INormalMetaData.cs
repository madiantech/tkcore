using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.MetaData
{
    public interface INormalMetaData : IMetaData
    {
        bool Single { get; }

        INormalTableData TableData { get; }

        IEnumerable<INormalTableData> TableDatas { get; }
    }
}