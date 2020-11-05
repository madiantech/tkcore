using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ProcessXmlConfig(NamespaceType = NamespaceType.Toolkit, RegName = WorkflowWebConst.APPROVE, Author = "YJC",
        CreateDate = "2018-04-04", Description = "给配置的ProcessXml添加审批的模块")]
    internal class ApproveProcessConfig : IConfigCreator<ProcessXml>
    {
        #region IConfigCreator<ProcessXml> 成员

        public ProcessXml CreateObject(params object[] args)
        {
            ProcessXml xml = Process != null ? Process.CreateObject() : null;
            return new ApproveProcessXml(xml);
        }

        #endregion IConfigCreator<ProcessXml> 成员

        [DynamicElement(ProcessXmlConfigFactory.REG_NAME)]
        public IConfigCreator<ProcessXml> Process { get; private set; }
    }
}