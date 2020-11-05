using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public abstract class Creator
    {
        private const int WFNNAME_LENGTH = 50;
        private string fWorkflowName;

        protected Creator()
        {
        }

        public virtual string WorkflowName
        {
            get
            {
                return fWorkflowName;
            }
            set
            {
                fWorkflowName = StringUtil.TruncString(value, WFNNAME_LENGTH);
            }
        }

        public WorkflowPriority Priority { get; set; }

        public abstract void AddContent(IDbDataSource source, WorkflowContent content, IParameter parameter);

        public abstract void SetWorkflowName(DataRow mainRow, IDbDataSource source);

        public FillContentMode FillMode { get; set; }
    }
}