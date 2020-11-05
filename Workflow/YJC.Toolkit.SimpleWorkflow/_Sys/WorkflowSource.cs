using System;
using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WorkflowSource : IDbDataSource, IDisposable
    {
        private readonly TkDbContext fContext;
        private readonly DataSet fDataSet;
        private readonly bool fFreeContext;

        public WorkflowSource(TkDbContext context)
            : this(context, true)
        {
        }

        public WorkflowSource(TkDbContext context, bool freeContext)
        {
            fContext = context;
            fFreeContext = freeContext;
            fDataSet = new DataSet(ToolkitConst.TOOLKIT) { Locale = ObjectUtil.SysCulture };
        }

        #region IWorkflowSource 成员

        public TkDbContext Context
        {
            get
            {
                return fContext;
            }
        }

        #endregion IWorkflowSource 成员

        #region IDataSource 成员

        public DataSet DataSet
        {
            get
            {
                return fDataSet;
            }
        }

        #endregion IDataSource 成员

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                fDataSet.Dispose();
                if (fFreeContext)
                    fContext.Dispose();
            }
        }
    }
}