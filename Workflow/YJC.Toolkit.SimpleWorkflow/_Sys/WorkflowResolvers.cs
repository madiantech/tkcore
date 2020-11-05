using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WorkflowResolvers : IDisposable
    {
        private readonly List<TableResolver> fTableResolvers;
        private readonly TkDbContext fContext;

        public WorkflowResolvers(TkDbContext context)
        {
            ApplyData = null;
            fContext = context;
            fTableResolvers = new List<TableResolver>();
        }

        public WorkflowResolvers(TkDbContext context, IEnumerable<TableResolver> tableResolvers)
            : this(context)
        {
            //fTableResolvers = new List<TableResolver>();
            fTableResolvers.AddRange(tableResolvers);
        }

        //public bool HasMainResolvers
        //{
        //    get;
        //    set;
        //}

        public Action<Transaction> ApplyData { get; set; }

        public IEnumerable<TableResolver> TableResolvers
        {
            get
            {
                return fTableResolvers;
            }
        }

        public void AddResolvers(IEnumerable<TableResolver> tableResolvers)
        {
            fTableResolvers.AddRange(tableResolvers);
        }

        public void AddResolver(TableResolver tableResolver)
        {
            fTableResolvers.Add(tableResolver);
        }

        public void Commit()
        {
            UpdateUtil.UpdateTableResolvers(fContext, ApplyData,
                fTableResolvers.Distinct(TableResolverEqualityComparer.Comparer));
        }

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
                ApplyData = null;
                foreach (var item in fTableResolvers)
                    item.DisposeObject();
            }
        }
    }
}