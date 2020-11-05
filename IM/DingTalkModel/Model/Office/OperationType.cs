using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public enum OperationType
    {
        [EnumFieldValue("EXECUTE_TASK_NORMAL")]
        ExecuteTaskNormal,

        [EnumFieldValue("EXECUTE_TASK_AGENT")]
        ExecuteTaskAgent,

        [EnumFieldValue("APPEND_TASK_BEFORE")]
        AppendTaskBefore,

        [EnumFieldValue("APPEND_TASK_AFTER")]
        AppendTaskAfter,

        [EnumFieldValue("REDIRECT_TASK")]
        RedirectTask,

        [EnumFieldValue("START_PROCESS_INSTANCE")]
        StartProcessInstance,

        [EnumFieldValue("TERMINATE_PROCESS_INSTANCE")]
        TerminateProcessInstance,

        [EnumFieldValue("FINISH_PROCESS_INSTANCE")]
        FinishProcessInstance,

        [EnumFieldValue("ADD_REMARK")]
        AddRemark
    }
}