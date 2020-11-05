using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public interface ITableData
    {
        string TableName { get; }

        string TableDesc { get; }

        string JsonFields { get; }

        IFieldInfoEx NameField { get; }

        IEnumerable<Tk5FieldInfoEx> HiddenList { get; }

        IEnumerable<Tk5FieldInfoEx> DataList { get; }
    }
}