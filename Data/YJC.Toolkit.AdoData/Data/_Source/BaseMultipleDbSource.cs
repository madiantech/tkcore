using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseMultipleDbSource : BaseSingleDbSource
    {
        private readonly RegNameList<ChildTableInfo> fChildTables = new RegNameList<ChildTableInfo>();

        protected BaseMultipleDbSource()
        {
        }

        protected BaseMultipleDbSource(IBaseDbConfig config)
            : base(config)
        {
            IMultipleResolverConfig multiple = config.Convert<IMultipleResolverConfig>();
            if (multiple.ChildResolvers != null)
            {
                foreach (var item in multiple.ChildResolvers)
                {
                    ChildTableInfo info = new ChildTableInfo(this, item);
                    AddChildTable(info);
                }
            }
        }

        protected IEnumerable<ChildTableInfo> ChildTables
        {
            get
            {
                return fChildTables;
            }
        }

        protected IEnumerable<TableResolver> ChildResolvers
        {
            get
            {
                var result = from item in fChildTables
                             select item.Resolver;
                return result;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var item in fChildTables)
                    item.Dispose();
            }

            base.Dispose(disposing);
        }

        public void AddChildTable(ChildTableInfo childTable)
        {
            TkDebug.AssertArgumentNull(childTable, "childTable", this);

            fChildTables.Add(childTable);
        }
    }
}
