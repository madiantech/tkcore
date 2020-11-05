using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal static class WorkflowConst
    {
        public static readonly ReadSettings ReadSettings = new ReadSettings { Encoding = Encoding.Default };

        public static readonly WriteSettings WriteSettings = new WriteSettings { Encoding = Encoding.Default };
    }
}