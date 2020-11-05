using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Excel
{
    internal class TempSource : IDbDataSource
    {
        public TempSource(DataSet dataSet, EmptyDbDataSource source)
        {
            DataSet = dataSet;
            Context = source.Context;
        }

        #region IDbDataSource 成员

        public DataSet DataSet { get; private set; }

        public TkDbContext Context { get; private set; }

        #endregion IDbDataSource 成员
    }
}