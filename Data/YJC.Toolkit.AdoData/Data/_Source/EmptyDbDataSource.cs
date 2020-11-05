using System;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class EmptyDbDataSource : IDbDataSource, IDisposable
    {
        private TkDbContext fContext;

        public EmptyDbDataSource()
        {
            DataSet = new DataSet(ToolkitConst.TOOLKIT) { Locale = ObjectUtil.SysCulture };
        }

        #region IDbDataSource 成员

        public TkDbContext Context
        {
            get
            {
                if (fContext == null)
                    SetContext(DbContextUtil.CreateDefault());
                return fContext;
            }
            set
            {
                SetContext(value);
            }
        }

        public DataSet DataSet { get; private set; }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        private void SetContext(TkDbContext context)
        {
            if (fContext != null)
                fContext.Dispose();
            fContext = context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                fContext.DisposeObject();
                DataSet.Dispose();
            }
        }
    }
}
