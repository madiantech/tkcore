using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal static class WorkflowWebConst
    {
        public const string QUERY_STRING_ID = "WfId";

        public const string APPROVE = "Approve";

        public const string MYWORK_URL = "~/c/plugin/c/WfMyWork";

        public const string STATUS_URL = "~/c/~source/C/WfStatus?WfId={0}";

        public static readonly QName ROOT_TABLE = QName.Get("Table", ToolkitConst.NAMESPACE_URL);
    }
}