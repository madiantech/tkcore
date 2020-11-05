using System.Collections.Generic;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [OperatorsConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2017-06-19",
        Description = "在简单列表的基础上，根据工作流是否启动来决定修改和删除按钮是否显示")]
    internal class SimpleWorkflowListOperatorsConfig : IConfigCreator<IOperatorsConfig>, IOperatorsConfig
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
                    if (IsDialog)
                        fOperators = ReadOnly ? new OperatorConfig[0] : new OperatorConfig[] {
                            OperatorConfig.InsertDialogOperator, OperatorConfig.UpdateDialogOperator,
                            OperatorConfig.DeleteOperator };
                    else
                        fOperators = ReadOnly ? new OperatorConfig[0] : new OperatorConfig[] {
                            OperatorConfig.InsertOperator, OperatorConfig.UpdateOperator,
                            OperatorConfig.DeleteOperator };
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
        public bool ReadOnly { get; set; }
    }
}