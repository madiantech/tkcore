using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using System.Data;

namespace YJC.Toolkit.Right
{
    internal sealed class UserDataSource : IDbDataSource, IDisposable
    {
        public UserDataSource(DbContextConfig config)
        {
            Context = config.CreateDbContext();
            DataSet = new DataSet { Locale = ObjectUtil.SysCulture };
        }

        #region IDbDataSource 成员

        public DataSet DataSet { get; private set; }

        public TkDbContext Context { get; private set; }

        #endregion IDbDataSource 成员

        #region IDisposable 成员

        public void Dispose()
        {
            Context.Dispose();
            DataSet.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员
    }
}