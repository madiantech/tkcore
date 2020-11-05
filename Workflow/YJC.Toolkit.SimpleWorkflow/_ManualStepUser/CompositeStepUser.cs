using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class CompositeStepUser : IManualStepUser
    {
        private readonly IEnumerable<IManualStepUser> fStepUsers;

        public CompositeStepUser(IEnumerable<IManualStepUser> stepUsers)
        {
            TkDebug.AssertArgumentNull(stepUsers, "stepUsers", null);

            fStepUsers = stepUsers;
        }

        public CompositeStepUser(IEnumerable<IConfigCreator<IManualStepUser>> stepUsers)
        {
            TkDebug.AssertArgumentNull(stepUsers, "stepUsers", null);

            fStepUsers = from item in stepUsers
                         where item != null
                         select item.CreateObject();
        }

        #region IManualStepUser 成员

        public IEnumerable<string> GetUserList(WorkflowContent content,
            DataRow workflowRow, IDbDataSource source)
        {
            var multiple = from item in fStepUsers
                           where item != null
                           select item.GetUserList(content, workflowRow, source);
            var result = Enumerable.Empty<string>();
            foreach (var item in multiple)
                result = result.Union(item);

            return result.Distinct();
        }

        #endregion IManualStepUser 成员
    }
}