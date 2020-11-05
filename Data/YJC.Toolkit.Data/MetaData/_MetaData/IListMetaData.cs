using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.MetaData
{
    public interface IListMetaData : IMetaData
    {
        IListTableData TableData { get; }
    }
}