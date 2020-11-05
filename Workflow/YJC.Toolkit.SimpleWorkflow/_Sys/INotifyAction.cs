using System.Data;
using YJC.Toolkit.Data;
using YJC.Toolkit.Right;

namespace YJC.Toolkit.SimpleWorkflow
{
    public interface INotifyAction
    {
        void DoAction(IUser user, TkDbContext context, DataRow workflowRow, DataRow mainRow);
    }
}