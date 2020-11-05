using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    public abstract class AutoProcessor : BaseProcessor
    {
        protected AutoProcessor()
        {
        }

        public FillContentMode FillMode { get; set; }

        public bool IsCreateSubWorkflow { get; set; }

        public abstract IEnumerable<TableResolver> Execute(DataRow workflowRow);
    }
}