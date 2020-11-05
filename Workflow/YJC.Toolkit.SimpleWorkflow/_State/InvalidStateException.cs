using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [Serializable]
    internal class InvalidStateException : ToolkitException
    {
        public InvalidStateException(object state)
            : base(string.Format(ObjectUtil.SysCulture,
            "当前对象的状态是{0}，属于无效状态", state), state)
        {
        }
    }
}