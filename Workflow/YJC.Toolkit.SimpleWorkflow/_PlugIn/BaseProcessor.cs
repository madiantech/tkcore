using System.Data;
using System.Transactions;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    public abstract class BaseProcessor
    {
        protected BaseProcessor()
        {
        }

        public StepConfig Config { get; internal set; }

        public IDbDataSource Source { get; internal set; }

        public WorkflowContent Content { get; internal set; }

        public virtual bool AddContent()
        {
            return false;
        }

        public virtual void ApplyDatas(Transaction transaction)
        {
        }

        internal bool SaveContent(DataRow workflowRow)
        {
            if (AddContent())
            {
                workflowRow["ContentXml"] = Content.CreateXml();
                return true;
            }
            return false;
        }
    }
}