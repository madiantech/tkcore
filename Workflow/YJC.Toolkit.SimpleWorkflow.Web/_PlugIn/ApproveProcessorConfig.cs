using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ProcessorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-04-04",
        RegName = WorkflowWebConst.APPROVE, Description = "对当前任务进行审批的Processor（配合Approve的ProcessXml使用）")]
    internal class ApproveProcessorConfig : IConfigCreator<Processor>
    {
        #region IConfigCreator<Processor> 成员

        public Processor CreateObject(params object[] args)
        {
            ProcessXml config = ObjectUtil.ConfirmQueryObject<ProcessXml>(this, args);
            return new ApproveProcessor(config);
        }

        #endregion IConfigCreator<Processor> 成员
    }
}