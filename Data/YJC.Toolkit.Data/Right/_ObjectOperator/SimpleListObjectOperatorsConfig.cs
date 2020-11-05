using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [ObjectOperatorsConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-11-13", Description = "默认列表对象操作符配置")]
    public sealed class SimpleListObjectOperatorsConfig : IConfigCreator<IObjectOperatorsConfig>,
        IObjectOperatorsConfig
    {
        private SimpleListObjectOperateRightConfig fConfig;
        private OperatorConfig[] fOperators;

        #region IObjectOperatorsConfig 成员

        IConfigCreator<IObjectOperateRight> IObjectOperatorsConfig.Right
        {
            get
            {
                if (fConfig == null)
                    fConfig = new SimpleListObjectOperateRightConfig
                    {
                        Operators = Operators
                    };
                return fConfig;
            }
        }

        IEnumerable<OperatorConfig> IObjectOperatorsConfig.Operators
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

        #endregion

        #region IConfigCreator<IObjectOperatorsConfig> 成员

        public IObjectOperatorsConfig CreateObject(params object[] args)
        {
            return this;
        }

        #endregion

        [SimpleAttribute(DefaultValue = UpdateKind.All)]
        public UpdateKind Operators { get; set; }

        [SimpleAttribute]
        public bool IsDialog { get; set; }
    }
}
