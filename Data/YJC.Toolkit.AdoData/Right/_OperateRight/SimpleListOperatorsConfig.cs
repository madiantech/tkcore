using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperatorsConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-11-27", Description = "默认列表操作符配置")]
    internal sealed class SimpleListOperatorsConfig : IConfigCreator<IOperatorsConfig>, IOperatorsConfig
    {
        private SimpleListOperateRightConfig fConfig;
        private OperatorConfig[] fOperators;

        #region IOperatorsConfig 成员

        IConfigCreator<IOperateRight> IOperatorsConfig.Right
        {
            get
            {
                if (fConfig == null)
                    fConfig = new SimpleListOperateRightConfig
                    {
                        Operators = Operators
                    };
                return fConfig;
            }
        }

        IEnumerable<OperatorConfig> IOperatorsConfig.Operators
        {
            get
            {
                if (fOperators == null)
                {
                    if (IsDialog)
                        fOperators = new OperatorConfig[] { OperatorConfig.InsertDialogOperator,
                            OperatorConfig.UpdateDialogOperator, OperatorConfig.DeleteOperator };
                    else
                        fOperators = new OperatorConfig[] { OperatorConfig.InsertOperator,
                            OperatorConfig.UpdateOperator, OperatorConfig.DeleteOperator };
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

        [SimpleAttribute(DefaultValue = UpdateKind.All)]
        public UpdateKind Operators { get; set; }

        [SimpleAttribute]
        public bool IsDialog { get; set; }
    }
}