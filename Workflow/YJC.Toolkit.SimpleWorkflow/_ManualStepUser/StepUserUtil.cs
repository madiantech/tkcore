using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Right;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal static class StepUserUtil
    {
        public static IEnumerable<string> Convert(IEnumerable<IUser> users)
        {
            var result = from user in users
                         select user.Id;
            return result;
        }
    }
}