using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [OperatorsConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2017-06-19",
        Description = "在详情的基础上，追加启动发送工作流的按钮，并根据工作流是否启动来决定这些按钮是否显示")]
    internal class SimpleWorkflowDetailOperatorsConfig : IConfigCreator<IOperatorsConfig>, IOperatorsConfig
    {
        private CreateWorkflowOperateRightConfig fConfig;
        private OperatorConfig[] fOperators;

        #region IOperatorsConfig 成员

        public IConfigCreator<IOperateRight> Right
        {
            get
            {
                if (fConfig == null)
                    fConfig = new CreateWorkflowOperateRightConfig
                    {
                        WfPrefix = WfPrefix,
                        ReadOnly = ReadOnly
                    };
                return fConfig;
            }
        }

        public IEnumerable<OperatorConfig> Operators
        {
            get
            {
                if (fOperators == null)
                {
                    var wfcreator = new WfCreateOperatorConfig(WorkflowName, IconClass, Caption);
                    var creator = wfcreator.CreateObject();
                    fOperators = ReadOnly ? new OperatorConfig[] { creator } : new OperatorConfig[] {
                        IsDialog ? OperatorConfig.UpdateDialogOperator : OperatorConfig.UpdateOperator,
                        creator};
                }
                return fOperators;
            }
        }

        #endregion IOperatorsConfig 成员

        #region IConfigCreator<IOperatorsConfig> 成员

        public IOperatorsConfig CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IOperatorsConfig> 成员

        [SimpleAttribute]
        public bool IsDialog { get; set; }

        [SimpleAttribute]
        public string WfPrefix { get; set; }

        [SimpleAttribute]
        public string IconClass { get; set; }

        [SimpleAttribute(Required = true)]
        public string WorkflowName { get; set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText Caption { get; set; }

        [SimpleAttribute]
        public bool ReadOnly { get; set; }
    }
}