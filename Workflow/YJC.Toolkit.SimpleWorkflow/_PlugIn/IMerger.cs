using System.Data;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.SimpleWorkflow
{
    public interface IMerger
    {
        bool IsWait(int workflowId, DataRow workflowRow, DataRow mainRow, IDbDataSource source);
    }
}